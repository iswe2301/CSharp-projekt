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
        public async Task<Account> CreateAccount(string accountNumber, string ownerPersonalNumber, string accountType, decimal initialBalance)
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

                await accountsCollection.InsertOneAsync(newAccount); // Lägger till kontot i databasen
                return newAccount; // Returnerar det nya kontot
            }
            catch (MongoException ex) // Fångar upp eventuella fel
            {
                Console.WriteLine($"Ett fel uppstod vid skapandet av kontot: {ex.Message}");
                throw;
            }
        }

        // Metod för att hämta kunds konton baserat på kontonummer
        public async Task<List<Account>> GetAccountsByUser(string personalNumber)
        {
            try
            {
                var accounts = await accountsCollection.Find(a => a.OwnerPersonalNumber == personalNumber).ToListAsync(); // Hämtar konton baserat på personnummer, returnerar en lista

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
        public async Task<bool> Deposit(string accountNumber, decimal amount)
        {
            try
            {
                var account = await accountsCollection.Find(a => a.AccountNumber == accountNumber).FirstOrDefaultAsync(); // Hämtar kontot från databasen

                // Kontrollerar om kontot finns
                if (account != null)
                {
                    account.Balance += amount; // Lägger till insättningen på kontot
                    await accountsCollection.ReplaceOneAsync(a => a.AccountNumber == accountNumber, account); // Uppdaterar kontot i databasen
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
        public async Task<bool> Withdraw(string accountNumber, decimal amount)
        {
            try
            {
                var account = await accountsCollection.Find(a => a.AccountNumber == accountNumber).FirstOrDefaultAsync(); // Hämtar kontot från databasen

                // Kontrollerar om kontot finns och om det finns tillräckligt med pengar på kontot
                if (account != null && account.Balance >= amount)
                {
                    account.Balance -= amount; // Drar av beloppet från kontots saldo
                    await accountsCollection.ReplaceOneAsync(a => a.AccountNumber == accountNumber, account); // Uppdaterar kontot i databasen
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
    }
}