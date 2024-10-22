using MongoDB.Driver;

namespace BankApp
{
    // Klass för att hantera transaktioner
    public class TransactionService
    {
        private readonly IMongoCollection<Transaction> transactionsCollection; // Kollektion för transaktioner

        // Konstruktor för att skapa en instans av TransactionService och hämta transaktioner från databasen
        public TransactionService()
        {
            var database = new Database();
            transactionsCollection = database.GetTransactionsCollection();
        }

        // Metod för att lägga till en ny transaktion i databasen
        public async Task AddTransaction(string accountNumber, string transactionType, decimal amount)
        {
            try
            {
                var transaction = new Transaction(accountNumber, transactionType, amount); // Skapar en ny transaktion
                await transactionsCollection.InsertOneAsync(transaction); // Lägger till transaktionen i databasen
            }
            catch (MongoException ex) // Fångar upp eventuella fel
            {
                Console.WriteLine($"Ett fel uppstod vid tillägg av transaktionen: {ex.Message}");
                throw;
            }
        }

        // Metod för att hämta alla transaktioner för ett specifikt konto
        public async Task<List<Transaction>> GetTransactionsByAccount(string accountNumber)
        {
            try
            {
                return await transactionsCollection.Find(t => t.AccountNumber == accountNumber).ToListAsync(); // Hämtar transaktioner baserat på kontonummer, returnerar en lista
            }
            catch (MongoException ex) // Fångar upp eventuella fel
            {
                Console.WriteLine($"Ett fel uppstod vid hämtning av transaktioner: {ex.Message}");
                return new List<Transaction>(); // Returnerar en tom lista om något går fel
            }
        }
    }
}