# ProcessPulse

![LOgo aplikacji](https://github.com/KospiPL/ProcessPulse/assets/105883537/aa3daff9-6351-4e8c-a91d-1479f4804d20)



Aplikacja do monitorowania procesów na różnych terminalach, składająca się z trzech głównych komponentów: REST API, Serwer oraz Klient (aplikacja okienkowa).

## Spis treści
- [Wymagania](#wymagania)
- [Instalacja](#instalacja)
- [Użycie](#użycie)
- [Funkcje](#funkcje)
- [Wsparcie](#wsparcie)
- [Licencja](#licencja)
- [Autor](#autor)
- [Podziękowania](#podziękowania)

## Wymagania
- .NET Framework 4.8
- .NET Core 3.1
- Entity Framework
- Windows Forms
- PowerShell
- SQL Server

[Przeczytaj więcej o instalacji](link_do_instrukcji_instalacji.md)

## Instalacja
1. Sklonuj repozytorium na swój lokalny komputer:
    ```bash
    git clone https://github.com/KospiPL/Client_Server_App.git
    ```
2. Otwórz rozwiązanie w Visual Studio.
3. Zainstaluj wszystkie wymagane pakiety NuGet:
    ```bash
    dotnet restore
    ```
4. Zaktualizuj connection string w `appsettings.json` dla bazy danych.
5. Uruchom migracje bazy danych:
    ```bash
    dotnet ef database update
    ```

## Użycie
1. **Serwer** - Uruchom serwis Windows, który zacznie zbierać dane o procesach.
2. **REST API** - Uruchom, aby umożliwić komunikację między serwerem a klientem.
3. **Klient** - Uruchom aplikację okienkową i użyj przycisku "Fetch Data", aby pobrać i wyświetlić dane o procesach.

[Zobacz więcej przykładów użycia](link_do_dokumentacji.md)

## Funkcje
- Zbieranie danych o procesach w czasie rzeczywistym.
- Możliwość przeglądania danych o procesach na różnych terminalach.
- Zapisywanie danych w bazie danych.
- Możliwość rozszerzenia o dodatkowe funkcje, takie jak alerty czy raporty.

## Wsparcie
W razie problemów lub pytań skontaktuj się ze mną poprzez [Kacper.cudzik11@gmail.com](mailto:kacper.cudzik11@gmail.com).

## Licencja
[MIT](LICENSE)

## Autor
Kacper Cudzik

## Podziękowania
Podziękowania dla wszystkich, którzy przyczynili się do tego projektu!
