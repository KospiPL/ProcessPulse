# ProcessPulse

![Aplikacja ProcessPulse](https://github.com/KospiPL/ProcessPulse/assets/105883537/f11db89e-3d94-4436-9ad6-241b6364a6f6)


Aplikacja ProcessPulse to kompleksowe narzędzie do monitorowania procesów na różnych terminalach. Składa się z kilku głównych komponentów, w tym REST API, serwisu Windows, biblioteki klas oraz interfejsu użytkownika w postaci aplikacji .NET MAUI Blazor.

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
- .NET 7.0
- Usługa Windows
- Entity Framework
- .NET MAUI Blazor
- SQL Server

[Więcej informacji na temat wymagań](link_do_instrukcji_instalacji.md)

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
6. Zainstaluj usługę Windows za pomocą polecenia:
    ```bash
    sc create "NazwaTwojejUsługi" binPath= "C:\ ścieżka do pilku ProcessPulse.ServiceServer.exe"
    ```

## Użycie
1. **Serwer** - Uruchom serwis Windows, który zbiera informacje o procesach na terminalach.
2. **REST API** - Uruchom API umożliwiające komunikację między serwerem a klientem.
3. **Klient** - Uruchom aplikację .NET MAUI Blazor i użyj przycisku "Fetch Data", aby pobierać i wyświetlać informacje o procesach.

[Zobacz przykłady użycia](link_do_dokumentacji.md)

## Funkcje
- Monitorowanie procesów w czasie rzeczywistym.
- Przeglądanie danych na różnych terminalach.
- Zapisywanie danych w bazie.
- Edycja terminali i procesów.

## Wsparcie
W razie problemów lub pytań skontaktuj się z autorem: [Kacper.cudzik11@gmail.com](mailto:kacper.cudzik11@gmail.com).

## Licencja
[MIT](LICENSE)

## Autor
Kacper Cudzik

## Podziękowania
Dziękujemy wszystkim, którzy przyczynili się do tego projektu!
