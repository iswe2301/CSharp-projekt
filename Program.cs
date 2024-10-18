using System;

namespace BankApp
{
    // Huvudklass för att köra programmet
    class Program
    {
        static void Main(string[] args)
        {

            var userService = new UserService(); // Skapar en instans av UserService
            var authService = new AuthService(userService); // Skapar en instans av AuthService, skickar med UserService
            var accountService = new AccountService();  // Skapar en instans av AccountService
            var transactionService = new TransactionService(); // Skapar en instans av TransactionService
            User? user = null; // Variabel för att lagra användaren

            // Anropar metod för att visa autentiseringsmenyn, skickar med instanser av UserService, AuthService, AccountService, TransactionService och en referens till användaren
            ShowAuthMenu(userService, authService, accountService, transactionService, ref user);
        }

        // Hanterar inloggning och registrering
        static void ShowAuthMenu(UserService userService, AuthService authService, AccountService accountService, TransactionService transactionService, ref User? user)
        {
            while (true)
            {
                // Rensar konsolen
                Console.Clear();
                Console.WriteLine("ISA Banken AB");
                Console.WriteLine("\n1. Ny kund? Registrera dig här.");
                Console.WriteLine("2. Befintlig kund? Logga in här.");
                Console.Write("\nAnge ditt val (1 eller 2): ");
                string? choice = Console.ReadLine(); // Läser in användarens val

                // Hanterar användarens val
                switch (choice)
                {
                    case "1":
                        // Anropar metod för att registrera ny användare
                        user = RegisterNewUser(userService);
                        // Kontrollerar om användaren registrerades
                        if (user != null)
                        {
                            Console.WriteLine("Registrering lyckades. Loggar in...");
                            System.Threading.Thread.Sleep(2000); // Väntar 2 sekunder
                            var mainMenu = new MainMenu(); // Instansierar huvudmenyn och skickar den vidare
                            mainMenu.ShowMainMenu(accountService, transactionService, user); // Visar huvudmenyn för inloggad användare
                        }
                        break;
                    case "2":
                        // Anropar metod för att logga in befintlig användare
                        user = LoginUser(authService, userService);
                        // Kontrollerar om användaren loggades in
                        if (user != null)
                        {
                            Console.WriteLine("Loggar in...");
                            System.Threading.Thread.Sleep(1000); // Väntar 1 sekund
                            var mainMenu = new MainMenu(); // Instansierar huvudmenyn och skickar den vidare
                            mainMenu.ShowMainMenu(accountService, transactionService, user); // Visar huvudmenyn för inloggad användare
                        }
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val. Ange 1 för att registrera dig eller 2 för att logga in."); // Felmeddelande vid ogiltigt val
                        System.Threading.Thread.Sleep(2000); // Väntar 2 sekunder så att användaren hinner läsa meddelandet
                        break;
                }
            }
        }

        // Metod för att registrera ny användare
        static User? RegisterNewUser(UserService userService)
        {
            // Försöker registrera en ny användare
            try
            {
                Console.Clear(); // Rensar konsolen
                Console.WriteLine("ISA Banken - Registrering");

                // Frågar användaren om personuppgifter för att registrera en ny användare
                string? personalNumber = InputValidation.GetValidPersonalNumber(userService); // Anropar metod för att hämta giltigt personnummer
                if (personalNumber == null) return null; // Återgår om användaren tryckte på X

                string? firstName = InputValidation.GetValidInput("Ange ditt förnamn"); // Hämtar förnamn
                if (firstName == null) return null; // Återgår om användaren tryckte på X

                string? lastName = InputValidation.GetValidInput("Ange ditt efternamn"); // Hämtar efternamn
                if (lastName == null) return null; // Återgår om användaren tryckte på X

                string? password = InputValidation.GetValidPassword(true); // Anropar metod för att hämta giltigt lösenord
                if (password == null) return null; // Återgår om användaren tryckte på X

                // Registrerar användaren i databasen 
                var user = userService.RegisterNewUser(personalNumber, firstName, lastName, password);

                // Kontrollerar om användaren redan finns
                if (user == null)
                {
                    Console.WriteLine("Fel: Du har redan ett befintligt kundkonto. Välj att logga in istället.");
                    Console.WriteLine("Tryck på valfri knapp för att återgå till huvudmenyn...");
                    Console.ReadKey(); // Väntar på att användaren trycker på en knapp innan huvudmenyn visas igen
                }
                return user; // Returnerar den nya användaren om registreringen lyckades
            }
            // Fångar upp eventuella fel vid registrering
            catch (Exception ex)
            {
                Console.WriteLine($"Ett fel uppstod vid registreringen: {ex.Message}");
                return null; // Returnerar null om registreringen misslyckades
            }
        }

        // Metod för att logga in befintlig användare
        static User? LoginUser(AuthService authService, UserService userService)
        {
            // Försöker logga in användaren
            try
            {
                Console.Clear(); // Rensar konsolen
                Console.WriteLine("ISA Banken - Logga in");

                // Anropar metod för att hämta giltigt personnummer
                string? personalNumber = InputValidation.GetValidPersonalNumber(userService);
                if (personalNumber == null) return null; // Återgår till huvudmenyn om användaren trycker på X

                AuthService.LoginStatus loginStatus; // Variabel för att lagra inloggningsstatus
                User? user = null; // Variabel för att lagra inloggad användare

                // Loopar tills användaren loggar in
                do
                {
                    string? passwordInput = InputValidation.GetValidPassword(false); // Anropar metod för att hämta giltigt lösenord, kontrollerar inte längd
                    if (passwordInput == null) return null; // Återgår till huvudmenyn om användaren trycker på X

                    (loginStatus, user) = authService.LoginUser(personalNumber, passwordInput); // Försöker logga in användaren

                    // Kontrollerar inloggningsstatus
                    if (loginStatus == AuthService.LoginStatus.UserNotFound)
                    {
                        Console.WriteLine("Inget kundkonto hittades. Välj att registrera dig istället.");
                        Console.WriteLine("Tryck på valfri knapp för att återgå till huvudmenyn...");
                        Console.ReadKey(); // Väntar på knapptryckning
                        return null; // Returnerar null om ingen användare hittas
                    }
                    else if (loginStatus == AuthService.LoginStatus.WrongPassword)
                    {
                        Console.WriteLine("Fel lösenord. Försök igen.");
                    }

                } while (loginStatus != AuthService.LoginStatus.Success); // Fortsätter tills inloggningen lyckas

                return user; // Returnerar inloggad användare
            }
            // Fångar upp eventuella fel vid inloggning
            catch (Exception ex)
            {
                Console.WriteLine($"Ett fel uppstod vid inloggningen: {ex.Message}");
                return null; // Returnerar null om inloggningen misslyckades
            }
        }
    }
}