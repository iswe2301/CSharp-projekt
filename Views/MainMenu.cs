using System;

namespace BankApp
{
    // Klass för att hantera inloggad meny och dess funktionalitet
    public class MainMenu
    {
        // Metod för att visa huvudmenyn i inloggat läge
        public void ShowMainMenu(AccountService accountService, TransactionService transactionService, User? user)
        {
            while (true)
            {
                // Rensar konsolen för huvudmenyn
                Console.Clear();
                Console.WriteLine("ISA Banken - Inloggad");
                Console.WriteLine($"Välkommen {user?.FirstName}!");
                ShowMenuOptions(); // Anropar metoden för att visa menyval
                string? choice = Console.ReadLine();

                // Hanterar användarens val i huvudmenyn
                switch (choice)
                {
                    case "1":
                        CreateNewAccount(accountService, transactionService, user!.PersonalNumber); // Anropar metoden för att skapa nytt konto
                        break;
                    case "2":
                        DepositMoney(accountService, transactionService, user!.PersonalNumber); // Anropar metod för att sätta in pengar
                        break;
                    case "3":
                        WithdrawMoney(accountService, transactionService, user!.PersonalNumber); // Anropar metod för att ta ut pengar
                        break;
                    case "4":
                        ShowAccounts(accountService, user!.PersonalNumber); // Anropar metod för att visa konton
                        break;
                    case "5":
                        ShowTransactions(accountService, transactionService, user!.PersonalNumber); // Anropa metod för att visa transaktioner
                        break;
                    case "6":
                        return; // Avbryter och återgår till huvudmenyn för att logga in eller registrera en ny användare
                    default:
                        Console.WriteLine("Ogiltigt val, försök igen."); // Felmeddelande vid ogiltigt val
                        System.Threading.Thread.Sleep(2000); // Väntar 2 sekunder så att användaren hinner läsa meddelandet
                        continue;
                }
            }
        }

        // Metod för att visa menyval
        private void ShowMenuOptions()
        {
            Console.WriteLine("\nMENY");
            Console.WriteLine("1. Skapa konto");
            Console.WriteLine("2. Insättning");
            Console.WriteLine("3. Uttag");
            Console.WriteLine("4. Mina konton");
            Console.WriteLine("5. Transaktioner");
            Console.WriteLine("6. Logga ut");
            Console.Write("\nVälj ett alternativ (1-6): ");
        }
    }
}