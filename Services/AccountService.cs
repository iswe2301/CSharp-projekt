using MongoDB.Driver;

namespace BankApp
{
    // Klass för att hantera konton/insättningar/uttag
    public class AccountService
    {
        // Kollektionen som hanterar konton i databasen
        private readonly IMongoCollection<Account> accountsCollection;

        // Konstruktor för att skapa en instans av AccountService och hämta kontokollektionen från databasen
        public AccountService()
        {
            var database = new Database();
            accountsCollection = database.GetAccountsCollection();
        }

        // Metod för att skapa ett nytt konto
        public Account CreateAccount(string accountNumber, string ownerPersonalNumber, string accountType, decimal initialBalance)
        {
            try
            {
                // Skapar en ny instans av Account och sätter värdena till de som skickats in
                var newAccount = new Account
                {
                    AccountNumber = accountNumber,
                    OwnerPersonalNumber = ownerPersonalNumber,
                    AccountType = accountType,
                    Balance = initialBalance
                };

                accountsCollection.InsertOne(newAccount); // Lägger till kontot i databasen
                return newAccount; // Returnerar det nya kontot
            }
            catch (MongoException ex) // Fångar upp eventuella fel
            {
                Console.WriteLine($"Ett fel uppstod vid skapandet av kontot: {ex.Message}");
                throw;
            }
        }

        // Metod för att hämta kunds konton baserat på kontonummer
        public List<Account> GetAccountsByUser(string personalNumber)
        {
            try
            {
                var accounts = accountsCollection.Find(a => a.OwnerPersonalNumber == personalNumber).ToList(); // Hämtar konton baserat på personnummer, returnerar en lista

                // Kontrollerar om inga konton finns och skriver ut meddelande
                if (accounts == null || !accounts.Any())
                {
                    Console.WriteLine("\nDu har inga befintliga konton.");
                    Console.WriteLine("\nTryck på valfri knapp för att återgå till huvudmenyn...");
                    Console.ReadKey();
                    return new List<Account>(); // Returnerar en tom lista om inga konton finns
                }
                    return accounts; // Returnerar listan med konton
            }
            catch (MongoException ex) // Fångar upp eventuella fel
            {
                Console.WriteLine($"Ett fel uppstod vid hämtning av konton: {ex.Message}");
                return new List<Account>(); // Returnerar en tom lista om något går fel
            }
        }

        // Metod för att sätta in pengar på ett konto (uppdatera befintligt saldo i db)
        public bool Deposit(string accountNumber, decimal amount)
        {
            try
            {
                var account = accountsCollection.Find(a => a.AccountNumber == accountNumber).FirstOrDefault(); // Hämtar kontot från databasen

                // Kontrollerar om kontot finns
                if (account != null)
                {
                    account.Balance += amount; // Lägger till insättningen på kontot
                    accountsCollection.ReplaceOne(a => a.AccountNumber == accountNumber, account); // Uppdaterar kontot i databasen
                    return true; // Returnerar true när insättningen lyckades
                }
                return false; // Returnerar false om kontot inte hittades
            }
            catch (MongoException ex) // Fångar upp eventuella fel
            {
                Console.WriteLine($"Ett fel uppstod vid insättningen: {ex.Message}");
                return false; // Returnerar false om något går fel
            }
        }

        // Metod för att göra ett uttag från ett konto (uppdatera befintligt saldo i db)
        public bool Withdraw(string accountNumber, decimal amount)
        {
            try
            {
                var account = accountsCollection.Find(a => a.AccountNumber == accountNumber).FirstOrDefault(); // Hämtar kontot från databasen

                // Kontrollerar om kontot finns och om det finns tillräckligt med pengar på kontot
                if (account != null && account.Balance >= amount)
                {
                    account.Balance -= amount; // Drar av beloppet från kontots saldo
                    accountsCollection.ReplaceOne(a => a.AccountNumber == accountNumber, account); // Uppdaterar kontot i databasen
                    return true; // Returnerar true när uttaget lyckades
                }
                return false; // Returnerar false om kontot inte hittades eller om det inte fanns tillräckligt med pengar
            }
            catch (MongoException ex) // Fångar upp eventuella fel
            {
                Console.WriteLine($"Ett fel uppstod vid uttaget: {ex.Message}");
                return false; // Returnerar false om något går fel
            }
        }

        // Metod för att skapa ett slumpmässigt kontonummer
        public string GenerateAccountNumber()
        {
            Random random = new Random(); // Skapar en instans av Random
            int randomNumber = random.Next(10000000, 99999999); // Slumpar ett nummer mellan 10000000 och 99999999, totalt 8 siffror
            return $"2034-{randomNumber}"; // Kombinerar clearingnummer med det slumpmässiga numret och returnerar
        }

        // Metod för att välja ett specifikt konto från en lista
        public Account SelectAccount(List<Account> accounts, string prompt)
        {
            Console.WriteLine("\nMina konton:");

            // Loopar igenom användarens konton och skriver ut dem
            for (int i = 0; i < accounts.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {accounts[i].AccountType} - {accounts[i].AccountNumber} (Saldo: {accounts[i].Balance:C})");
            }

            int selectedAccountIndex; // Variabel för att lagra det valda kontots index

            // Loopar tills användaren valt ett giltigt konto
            while (true)
            {
                Console.Write(prompt); // Skriver ut meddelandet som skickats in
                string? selectedAccountIndexInput = Console.ReadLine(); // Läser in användarens val

                // Kontrollerar att användaren valt ett giltigt konto
                if (int.TryParse(selectedAccountIndexInput, out selectedAccountIndex) && selectedAccountIndex >= 1 && selectedAccountIndex <= accounts.Count)
                {
                    return accounts[selectedAccountIndex - 1]; // Returnerar det valda kontot (-1 pga index)
                }
                else
                {
                    Console.WriteLine("Fel: Ogiltigt val. Försök igen."); // Skriver ut felmeddelande om valet inte är giltigt
                }
            }
        }
    }
}