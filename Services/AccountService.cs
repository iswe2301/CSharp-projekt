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
    }
}