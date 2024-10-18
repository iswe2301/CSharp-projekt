using System;

namespace BankApp
{
    // Klass för att hantera inloggad meny och dess funktionalitet
    public class MainMenu
    {
        // Metod för att visa huvudmenyn i inloggat läge
        public void ShowMainMenu(AccountService accountService, TransactionService transactionService, User? user)
        {
            var loanService = new LoanService(); // Skapar en instans av LoanService
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
                        MakeLoanApplication(loanService); // Anropar metod för att göra en låneansökan
                        break;
                    case "7":
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
            Console.WriteLine("6. Låneansökan");
            Console.WriteLine("7. Logga ut");
            Console.Write("\nVälj ett alternativ (1-7): ");
        }

        // Metod för att skapa nytt konto
        private void CreateNewAccount(AccountService accountService, TransactionService transactionService, string personalNumber)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("ISA Banken - Skapa konto");

                string? accountType = InputValidation.GetAccountType(); // Hämtar kontotyp
                if (accountType == null) return; // Återgår till huvudmenyn om kontotypen är null

                decimal? initialBalance = InputValidation.GetInitialBalance(); // Hämtar startbelopp
                if (initialBalance == null) return; // Återgår till huvudmenyn om startbeloppet är null

                string accountNumber = accountService.GenerateAccountNumber(); // Genererar ett nytt kontonummer
                var account = accountService.CreateAccount(accountNumber, personalNumber, accountType, initialBalance.Value); // Skapar ett nytt konto i databasen

                transactionService.AddTransaction(accountNumber, "Insättning vid kontoskapande", initialBalance.Value); // Skapar en transaktion för insättningen

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
                var selectedAccount = InputValidation.SelectAccount(accounts, "Välj vilket konto du vill sätta in pengar på (ange siffra) eller skriv 'X' för att avbryta: ");
                if (selectedAccount == null) return; // Återgår till huvudmenyn om kontot är null

                // Anropar metod för att validera och få giltigt belopp
                decimal? amount = InputValidation.GetValidAmount("Ange belopp att sätta in (minst 100 kr)", 100);
                if (amount == null) return; // Återgår till huvudmenyn om beloppet är null

                // Kontrollerar om insättningen lyckades i databasen
                if (accountService.Deposit(selectedAccount.AccountNumber!, amount.Value))
                {
                    transactionService.AddTransaction(selectedAccount.AccountNumber!, "Insättning", amount.Value); // Lägger till transaktionen i databasen
                    Console.WriteLine($"Insättning lyckades. Nytt saldo för {selectedAccount.AccountType}: {selectedAccount.Balance + amount.Value:C}"); // Skriver ut bekräftelse
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

        // Metod för att ta ut pengar från ett konto
        private void WithdrawMoney(AccountService accountService, TransactionService transactionService, string personalNumber)
        {
            try
            {
                Console.Clear(); // Rensar konsolen
                Console.WriteLine("ISA Banken - Uttag");

                // Hämtar alla användarens konton baserat på personnumret
                var accounts = accountService.GetAccountsByUser(personalNumber);

                // Kontrollerar om användaren har några konton, annars avslutas metoden
                if (!accounts.Any()) return;

                // Anropar metod för att välja konto
                var selectedAccount = InputValidation.SelectAccount(accounts, "Välj vilket konto du vill ta ut pengar från (ange siffra) eller skriv 'X' för att avbryta: ");
                if (selectedAccount == null) return; // Återgår till huvudmenyn om kontot är null

                decimal? amount; // Variabel för att lagra beloppet

                while (true) // Loopar tills ett giltigt belopp anges
                {
                    // Anropar metod för att validera och få giltigt belopp
                    amount = InputValidation.GetValidAmount("Ange belopp att ta ut (minst 100 kr): ", 100);
                    if (amount == null) return; // Återgår till huvudmenyn om beloppet är null

                    // Kontrollerar om beloppet är högre än kontots saldo
                    if (selectedAccount.Balance >= amount.Value)
                    {
                        // Kontrollerar om uttaget lyckades genomföras i databasen
                        if (accountService.Withdraw(selectedAccount.AccountNumber!, amount.Value))
                        {
                            selectedAccount.Balance -= amount.Value; // Uppdaterar saldot
                            transactionService.AddTransaction(selectedAccount.AccountNumber!, "Uttag", amount.Value); // Lägger till transaktionen i databasen
                            Console.WriteLine($"Uttag av {amount.Value:C} lyckades. Nytt saldo för {selectedAccount.AccountType}: {selectedAccount.Balance:C}");
                            break; // Bryter ut ur loopen efter lyckad transaktion
                        }
                    }
                    else
                    {
                        Console.WriteLine("Fel: Otillräckligt saldo. Försök igen.");
                    }
                }
                Console.WriteLine("Tryck på valfri knapp för att återgå till huvudmenyn...");
                Console.ReadKey();
            }
            catch (Exception ex) // Fångar upp eventuella fel
            {
                Console.WriteLine($"Ett fel uppstod vid uttaget: {ex.Message}");
                Console.WriteLine("Tryck på valfri knapp för att återgå till huvudmenyn...");
                Console.ReadKey();
            }
        }

        // Metod för att visa alla kundens konton
        private void ShowAccounts(AccountService accountService, string personalNumber)
        {
            try
            {
                Console.Clear(); // Rensar konsolen
                Console.WriteLine("ISA Banken - Mina konton");

                // Hämtar alla användarens konton baserat på personnumret
                var accounts = accountService.GetAccountsByUser(personalNumber);

                // Kontrollerar om användaren har några konton, annars avslutas metoden
                if (!accounts.Any()) return;

                // Loopar igenom användarens konton och skriver ut dem
                foreach (var account in accounts)
                {
                    Console.WriteLine($"{account.AccountType} - {account.AccountNumber} (Saldo: {account.Balance:C})");
                }

                // Väntar på att användaren trycker på en knapp innan den återgår till huvudmenyn
                Console.WriteLine("\nTryck på valfri knapp för att återgå till huvudmenyn...");
                Console.ReadKey();
            }
            catch (Exception ex) // Fångar upp eventuella fel
            {
                Console.WriteLine($"Ett fel uppstod vid visning av konton: {ex.Message}");
                Console.WriteLine("Tryck på valfri knapp för att återgå till huvudmenyn...");
                Console.ReadKey();
            }
        }

        // Metod för att visa transaktioner för ett specifikt konto
        private void ShowTransactions(AccountService accountService, TransactionService transactionService, string personalNumber)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("ISA Banken - Transaktioner");

                // Hämta alla konton för användaren baserat på personnumret
                var accounts = accountService.GetAccountsByUser(personalNumber);

                // Kontrollerar om användaren har några konton, annars avslutas metoden
                if (!accounts.Any()) return;

                // Anropar metod för att välja konto
                var selectedAccount = InputValidation.SelectAccount(accounts, "Välj ett konto för att visa transaktioner (ange siffra) eller skriv 'X' för att avbryta: ");
                if (selectedAccount == null) return; // Återgår till huvudmenyn om kontot är null

                // Hämtar transaktioner för valt konto
                var transactions = transactionService.GetTransactionsByAccount(selectedAccount.AccountNumber!);

                // Kontrollerar om det finns transaktioner för kontot
                if (transactions == null || !transactions.Any())
                {
                    Console.WriteLine($"\nInga transaktioner tillgängliga för {selectedAccount.AccountType} med kontonummer {selectedAccount.AccountNumber}");
                }
                else
                {
                    Console.Clear(); // Rensar konsolen
                    Console.WriteLine($"\nTransaktioner för {selectedAccount.AccountType} med kontonummer {selectedAccount.AccountNumber}");
                    Console.WriteLine();

                    // Loopar igenom transaktionerna och skriver ut dem
                    foreach (var transaction in transactions)
                    {
                        Console.WriteLine($"{transaction.Date} - {transaction.TransactionType}: {transaction.Amount:C}");
                    }
                }

                Console.WriteLine("\nTryck på valfri knapp för att återgå till huvudmenyn...");
                Console.ReadKey();
            }
            catch (Exception ex) // Fångar upp eventuella fel
            {
                Console.WriteLine($"Ett fel uppstod vid visning av transaktioner: {ex.Message}");
                Console.WriteLine("Tryck på valfri knapp för att återgå till huvudmenyn...");
                Console.ReadKey();
            }
        }

        // Metod för att göra en låneansökan
        private void MakeLoanApplication(LoanService loanService)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("ISA Banken - Låneansökan");
                Console.WriteLine("Fyll i ansökan med de uppgifter som efterfrågas så får du ett förhandsbesked på om ditt lån kommer att beviljas eller inte.");

                // Hämtar input från användaren och validerar den
                float monthlyIncome = InputValidation.GetValidMoneyInput("\nAnge din månadsinkomst (brutto) eller skriv 'X' för att avbryta: ", 0);
                if (monthlyIncome == -1) return; // Avbryter om användaren skriver X

                float monthlyExpenses = InputValidation.GetValidMoneyInput("Ange dina månatliga utgifter eller skriv 'X' för att avbryta: ", 0);
                if (monthlyExpenses == -1) return; // Avbryter om användaren skriver X

                float loanAmount = InputValidation.GetValidMoneyInput("Ange det belopp som du önskar låna (minst 10 000 kr) eller skriv 'X' för att avbryta: ", 10000);
                if (loanAmount == -1) return; // Avbryter om användaren skriver X

                string? hasFixedIncome = InputValidation.GetYesOrNoInput("Har du en fast månatlig inkomst? (Ja/Nej) eller skriv 'X' för att avbryta: ");
                if (hasFixedIncome == null) return; // Avbryter om användaren skriver X

                string? isEmployed = InputValidation.GetYesOrNoInput("Är du fast anställd på 100%? (Ja/Nej) eller skriv 'X' för att avbryta: ");
                if (isEmployed == null) return; // Avbryter om användaren skriver X

                string? hasCurrentLoan = InputValidation.GetYesOrNoInput("Har du befintliga lån idag? (Ja/Nej) eller skriv 'X' för att avbryta: ");
                if (hasCurrentLoan == null) return; // Avbryter om användaren skriver X

                string? hasDebtIssues = InputValidation.GetYesOrNoInput("Har du betalningsanmärkningar? (Ja/Nej) eller skriv 'X' för att avbryta: ");
                if (hasDebtIssues == null) return; // Avbryter om användaren skriver X

                // Anropar metoden för att förutsäga om lånet kommer att bli beviljat
                string prediction = loanService.PredictLoanApproval(monthlyIncome, monthlyExpenses, loanAmount, hasFixedIncome, isEmployed, hasCurrentLoan, hasDebtIssues);

                Console.WriteLine("Behandlar din ansökan..."); // Skriver ut meddelande om att ansökan behandlas
                System.Threading.Thread.Sleep(2000); // Väntar 2 sekunder så att användaren hinner läsa meddelandet
                Console.Clear(); // Rensar konsolen
                Console.WriteLine("ISA Banken - Låneansökan");
                Console.WriteLine("\nDitt förhandsbesked:");
                Console.WriteLine(prediction); // Skriver ut förutsägelsen

                Console.WriteLine("\nTryck på valfri knapp för att återgå till huvudmenyn...");
                Console.ReadKey();
            }
            catch (Exception ex) // Fångar upp eventuella fel
            {
                Console.WriteLine($"Ett fel uppstod vid låneansökan: {ex.Message}");
                Console.WriteLine("Tryck på valfri knapp för att återgå till huvudmenyn...");
                Console.ReadKey();
            }
        }
    }
}