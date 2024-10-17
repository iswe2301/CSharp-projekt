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
    }
}