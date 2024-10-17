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

        // Metod för att skapa nytt konto
        private void CreateNewAccount(AccountService accountService, TransactionService transactionService, string personalNumber)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("ISA Banken - Skapa konto");

                string accountType = InputValidation.GetAccountType(); // Hämtar kontotyp
                decimal initialBalance = InputValidation.GetInitialBalance(); // Hämtar startbelopp

                string accountNumber = accountService.GenerateAccountNumber(); // Genererar ett nytt kontonummer
                var account = accountService.CreateAccount(accountNumber, personalNumber, accountType, initialBalance); // Skapar ett nytt konto i databasen

                transactionService.AddTransaction(accountNumber, "Insättning vid kontoskapande", initialBalance); // Skapar en transaktion för insättningen

                Console.WriteLine($"{accountType} skapat med kontonummer {account.AccountNumber} och startbelopp {account.Balance:C}."); // Skriver ut bekräftelse
                Console.WriteLine("Tryck på valfri knapp för att återgå till huvudmenyn...");
                Console.ReadKey();
            }
            catch (Exception ex) // Fångar upp eventuella fel
            {
                Console.WriteLine($"Ett fel uppstod vid skapandet av kontot: {ex.Message}");
                Console.WriteLine("Tryck på valfri knapp för att återgå till huvudmenyn...");
                Console.ReadKey();
            }
        }

        // Metod för att sätta in pengar på ett konto
        private void DepositMoney(AccountService accountService, TransactionService transactionService, string personalNumber)
        {
            try
            {
                Console.Clear(); // Rensar konsolen
                Console.WriteLine("ISA Banken - Insättning");

                // Hämtar alla användarens konton baserat på personnumret
                var accounts = accountService.GetAccountsByUser(personalNumber);

                // Kontrollerar om användaren har några konton, annars avslutas metoden
                if (!accounts.Any()) return;

                // Anropar metod för att välja konto
                var selectedAccount = accountService.SelectAccount(accounts, "Välj vilket konto du vill sätta in pengar på (ange siffra): ");

                // Anropar metod för att validera och få giltigt belopp
                decimal amount = InputValidation.GetValidAmount("Ange belopp att sätta in (minst 100 kr): ", 100);

                // Kontrollerar om insättningen lyckades i databasen
                if (accountService.Deposit(selectedAccount.AccountNumber!, amount))
                {
                    transactionService.AddTransaction(selectedAccount.AccountNumber!, "Insättning", amount); // Lägger till transaktionen i databasen
                    Console.WriteLine($"Insättning lyckades. Nytt saldo för {selectedAccount.AccountType}: {selectedAccount.Balance + amount:C}"); // Skriver ut bekräftelse
                    Console.WriteLine("Tryck på valfri knapp för att återgå till huvudmenyn...");
                    Console.ReadKey();
                }
            }
            catch (Exception ex) // Fångar upp eventuella fel
            {
                Console.WriteLine($"Ett fel uppstod vid insättningen: {ex.Message}");
                Console.WriteLine("Tryck på valfri knapp för att återgå till huvudmenyn...");
                Console.ReadKey();
            }
        }
    }
}