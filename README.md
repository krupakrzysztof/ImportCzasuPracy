Rozwiązanie służy do importu czasu pracy do "Kalendarz/Norma czasu pracy".

Worker widoczny jest na liście pracowników w czynnościach i nazywa się "Import czasu pracy"

![image](https://github.com/krupakrzysztof/ImportCzasuPracy/assets/87368964/74b77a7a-b4ce-4366-92fc-faf903479564)

Prosi on o podanie ścieżki do pliku JSON z dniami pracy (przykład pliku poniżej). Po potwierdzeniu rozpoczenie się tworzenie dni kalendarza dla pracowników

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
![image](https://github.com/krupakrzysztof/ImportCzasuPracy/assets/87368964/20581954-0508-45d4-9fbc-b5b51fe94368)
