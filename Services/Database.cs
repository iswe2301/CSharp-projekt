using MongoDB.Driver;
using DotNetEnv;

namespace BankApp
{
    // Klass för att skapa en anslutning till databasen
    public class Database
    {
        private IMongoDatabase db; // Databasen som används för att spara data

        // Konstruktor för att skapa en anslutning till databasen
        public Database()
        {
            try
            {
                Env.Load(); // Laddar miljövariabler från .env-filen
                var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION"); // Hämtar anslutningssträngen från miljövariabeln
                var client = new MongoClient(connectionString); // Skapar en ny klient för att ansluta till MongoDB
                db = client.GetDatabase("BankAppDb"); // Hämtar databasen som ska användas
            }
            catch (MongoException ex)
            {
                Console.WriteLine($"Ett fel uppstod vid anslutning till databasen: {ex.Message}");
                throw;
            }
        }
    }
}