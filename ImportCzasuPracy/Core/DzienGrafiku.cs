using System;

namespace ImportCzasuPracy.Core
{
    public class DzienGrafiku
    {
        public string PracownikKod { get; set; }
        public DateTime Data { get; set; }
        public string DefinicjaDnia { get; set; }
        public TimeSpan Rozpoczecie { get; set; }
        public TimeSpan CzasPracy { get; set; }
    }
}
