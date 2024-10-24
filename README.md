# DT071G - Programmering i C#.NET, Moment 5 Projekt

## Projektbeskrivning - bankapplikation
Detta projekt är en konsolbaserad bankapplikation som låter användare registrera sig, logga in, hantera konton, utföra transaktioner samt ansöka om förhandsbesked för lån. Låneansökan använder en maskininlärningsmodell för att förutsäga om det är troligt att lånet kommer att bli beviljat eller inte.

Projektet har utvecklats som en del av kursen DT071G - Programmering i C#.NET.

## Om applikationen
Applikationen är skriven i C# med .NET och MongoDB för databashantering. Den använder ML.NET och Model Builder för att skapa en maskininlärningsmodell som används för att förutsäga lånebeslut. Användarinteraktionen hanteras via konsolen där användare kan skapa och hantera konton, utföra insättningar och uttag, samt ansöka om lån.

### Huvudfunktioner
Bankapplikationen består av följande huvudfunktioner:

* **Registrering och inloggning**: Användare kan registrera sig med personnummer, förnamn, efternamn och lösenord för att skapa ett nytt kundkonto. Befintliga användare kan istället välja att logga in med sitt personnummer och lösenord. Användaruppgifter sparas i databasen och lösenord hashas.
* **Skapa nytt bankkonto**: När en användare är inloggad kan de skapa nya bankkonton av olika typer, till exempel sparkonto eller lönekonto. När ett konto skapas genereras ett slumpmässigt kontonummer, samt att en insättning om minst 100 kronor måste göras i samband med skapandet.
* **Insättning och uttag**: Användare kan göra insättningar och uttag från sina konton, och transaktionerna loggas i databasen. Insättning och uttag måste göras om minst 100 kronor.
* **Visa konton**: Användare kan se alla sina konton med aktuella saldon.
* **Visa transaktioner**: Användare kan välja ett specifikt konto för att visa alla transaktioner på kontot, dessa inkluderar datum och typ av transaktion.
* **Låneansökan**: Användare kan ansöka om förhandsbesked för lån genom att ange sin inkomst, utgifter och andra relevanta uppgifter. Låneansökan använder en ML.NET-modell som förutsäger sannolikheten för att ett lån ska beviljas, baserat på användarens finansiella situation.
* **Logga ut**: Användare kan välja att logga ut från sitt konto och återvända till startmenyn för att logga in/registrera kundkonto.

### Arkitektur
Projektet är uppdelat i olika delar för att hålla koden organiserad och modulär:

- **Models**: Innehåller modellerna för `User`, `Account`, och `Transaction`, vilka representerar användarna och deras data i MongoDB.
- **Services**: `AccountService`, `AuthService`, `UserService`, och `TransactionService` hanterar logik för konton, autentisering, användare, och transaktioner.
- **MLModels**: Innehåller maskininlärningsmodellen för låneförutsägelse som genererats med ML.NET Model Builder.
- **Helpers**: `InputValidation` hanterar validering av användarens inmatning i konsolen.
- **Views**: `MainMenu.cs` hanterar användargränssnittet för interaktion med användaren när denne har loggat in (huvudmeny för inloggat läge).
- **Program.cs**: Huvudfilen som startar applikationen. Hanterar användargränssnittet för interaktion med användaren innan denna har loggat in (huvudmeny för utloggat läge).

### Tekniker och verktyg
- **VS och VS-Code**: Används som utvecklingsmiljöer för att bygga och testa applikationen.
- **C# & .NET**: Huvudspråket och ramverket som används för hela applikationen.
- **MongoDB**: Används för att spara användare, konton och transaktioner.
- **ML.NET & Model Builder**: Används för att bygga och träna en maskininlärningsmodell för låneansökningsförutsägelse.
- **DotNetEnv**: Används för att ladda miljövariabler från `.env`-filen som innehåller databaskonfiguration. I filen `.env example` finns instruktioner för hur applikationen kan konfigureras med en egen databasanslutning i en `.env`-fil.
- **PasswordHasher**: Används för att hasha och verifiera användarlösenord.

## Om
* **Av:** Isa Westling
* **Kurs:** DT071G Programmering i C#.NET
* **Program:** Webbutvecklingsprogrammet
* **År:** 2024
* **Skola:** Mittuniversitetet