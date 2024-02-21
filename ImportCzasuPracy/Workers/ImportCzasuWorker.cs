using ImportCzasuPracy.Core;
using ImportCzasuPracy.Workers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Soneta.Business;
using Soneta.Business.UI;
using Soneta.Kadry;
using Soneta.Kalend;
using Soneta.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: Worker(typeof(ImportCzasuWorker), typeof(Pracownicy))]

namespace ImportCzasuPracy.Workers
{
    public class ImportCzasuWorker : ISessionable
    {
        [Context]
        public Session Session { get; set; }

        [Context]
        public ImportCzasuWorkerParams Params { get; set; }

        private Log log = new Log("Import czasu pracy", true);

        [Action("Import czasu pracy",
            Description = "",
            Priority = 100,
            Target = ActionTarget.Menu,
            Mode = ActionMode.SingleSession | ActionMode.ConfirmSave | ActionMode.Progress)]
        public void Importuj()
        {
            KalendModule kalendModule = KalendModule.GetInstance(this);
            KadryModule kadryModule = KadryModule.GetInstance(this);
            IFileSystemService provider = Session.GetRequiredService<IFileSystemService>();
            string fileContent = Encoding.UTF8.GetString(provider.ReadFile(Params.FilePath));
            List<DzienGrafiku> dniGrafiku = JArray.Parse(fileContent).ToObject<List<DzienGrafiku>>();

            using (var trans = Session.Logout(true))
            {
                foreach (DzienGrafiku dzienGrafiku in dniGrafiku)
                {
                    Pracownik pracownik = kadryModule.Pracownicy.WgKodu[dzienGrafiku.PracownikKod];
                    if (pracownik == null)
                    {
                        log.Warning($"Nie znaleziono pracownika o kodzie {dzienGrafiku.PracownikKod}.");
                        continue;
                    }
                    if (pracownik.Kalendarze.Count != 1)
                    {
                        log.Warning($"Pracownik o kodzie {pracownik.Kod} posiada inną liczbę kalendarzy niż 1. Pomijam go.");
                        continue;
                    }
                    KalendarzBase kalendarz = pracownik.Kalendarze.First();
                    DefinicjaDnia definicja = kalendModule.DefinicjeDni.WgNazwy[dzienGrafiku.DefinicjaDnia];
                    if (definicja == null)
                    {
                        log.WriteLine($"Nie znaleziono definicji dnia o nazwie {dzienGrafiku.DefinicjaDnia} dla pracownika {pracownik.Kod} na {dzienGrafiku.Data.Date}");
                        continue;
                    }

                    KalkulatorPlanu kalkulatorPlanu = new KalkulatorPlanu(pracownik);
                    kalkulatorPlanu.LoadOkres(new FromTo(dzienGrafiku.Data, dzienGrafiku.Data));
                    Dzien dzienPlanu = kalkulatorPlanu[dzienGrafiku.Data];

                    if (dzienPlanu.Definicja.Typ == TypDnia.Pracy
                        && pracownik.Last.Etat.Kalendarz.DefinicjaDnia.Praca.OdGodziny == dzienGrafiku.Rozpoczecie.ToTime()
                        && pracownik.Last.Etat.Kalendarz.DefinicjaDnia.Praca.Czas == dzienGrafiku.CzasPracy.ToTime())
                    {
                        continue;
                    }
                    if ((dzienPlanu.Definicja.Typ == TypDnia.Świąteczny && definicja.Typ == TypDnia.Świąteczny)
                        || (dzienPlanu.Definicja.Typ == TypDnia.Wolny && definicja.Typ == TypDnia.Wolny))
                    {
                        continue;
                    }

                    DzienKalendarzaBase dzienKalendarza = kalendModule.DniKalendarza.WgKalendarz[kalendarz][new FieldCondition.Equal("Data", new Date(dzienGrafiku.Data))].FirstOrDefault();
                    if (dzienKalendarza == null)
                    {
                        dzienKalendarza = new DzienPlanu(pracownik, dzienGrafiku.Data);
                        kalendModule.DniKalendarza.AddRow(dzienKalendarza);
                    }
                    if (dzienKalendarza.Definicja == null || dzienKalendarza.Definicja.Nazwa != dzienGrafiku.DefinicjaDnia)
                    {
                        dzienKalendarza.Definicja = definicja;
                    }
                    if (dzienKalendarza.Definicja.Typ == TypDnia.Pracy)
                    {
                        if (dzienKalendarza.Praca.OdGodziny != dzienGrafiku.Rozpoczecie.ToTime())
                        {
                            dzienKalendarza.Praca.OdGodziny = dzienGrafiku.Rozpoczecie.ToTime();
                        }
                        if (dzienKalendarza.Praca.Czas != dzienGrafiku.CzasPracy.ToTime())
                        {
                            dzienKalendarza.Praca.Czas = dzienGrafiku.CzasPracy.ToTime();
                        }
                    }
                }

                trans.Commit();
            }
        }
    }


    public class ImportCzasuWorkerParams : ContextBase
    {
        public ImportCzasuWorkerParams(Context context) : base(context)
        {

        }

        [Required]
        public string FilePath { get; set; }

        public object GetListFilePath()
        {
            return new FileDialogInfo
            {
                Title = "Wybierz plik",
                DefaultExt = ".json",
                ForbidMultiSelection = true
            };
        }
    }
}
