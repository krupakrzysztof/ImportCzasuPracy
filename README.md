Rozwiązanie służy do importu czasu pracy do "Kalendarz/Norma czasu pracy".

Worker widoczny jest na liście pracowników w czynnościach i nazywa się "Import czasu pracy"

![image](https://github.com/krupakrzysztof/ImportCzasuPracy/assets/87368964/c7292a49-95e1-477d-bdac-00f468a4cdbc)


Prosi on o podanie ścieżki do pliku JSON z dniami pracy (przykład pliku poniżej). Po potwierdzeniu rozpoczenie się tworzenie dni kalendarza dla pracowników.
Weryfikuje on czy tworzony dzień jest zgodny ze standardowym dniem (zgadza się godzina rozpoczęcia pracy oraz czas jej trwania) i jeżeli tak to pominie jego dodanie.

Przykładowa zawartość pliku JSON do importu przez dodatek:
``` json
[
  {
    "PracownikKod": "006",
    "Data": "2024-01-13",
    "DefinicjaDnia": "Praca 8-20",
    "Rozpoczecie": "08:00:00",
    "CzasPracy": "12:00:00"
  },
  {
    "PracownikKod": "006",
    "Data": "2024-01-15",
    "DefinicjaDnia": "Praca 8-16",
    "Rozpoczecie": "08:00:00",
    "CzasPracy": "08:00:00"
  }
]
```
Po zaimportowaniu przykładowego pliku zakładka z normą czasu pracy wygląda tak:
![image](https://github.com/krupakrzysztof/ImportCzasuPracy/assets/87368964/e49882c8-05d6-4bbc-bd3c-cf636fb29a96)
