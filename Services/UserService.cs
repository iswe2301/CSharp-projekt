using MongoDB.Driver;
using System.Text.RegularExpressions; // Används för att kontrollera personnummer

namespace BankApp
{
    // Klass för att hantera användare
    public class UserService
    {
        private readonly IMongoCollection<User> usersCollection;

        // Konstruktor för att skapa en instans av UserService och hämta användare från databasen
        public UserService()
        {
            var database = new Database();
            usersCollection = database.GetUsersCollection();
        }

        // Metod för att registrera ny användare
        public async Task<User?> RegisterNewUser(string personalNumber, string firstName, string lastName, string password)
        {
            try
            {
                // Kontrollerar om användaren redan finns i databasen
                var existingUser = await usersCollection.Find(u => u.PersonalNumber == personalNumber).FirstOrDefaultAsync();
                if (existingUser != null)
                {
                    return null; // Returnerar null om användaren redan finns
                }

                var newUser = new User(personalNumber, firstName, lastName, password); // Skapar en ny användare
                await usersCollection.InsertOneAsync(newUser); // Lägger till användaren i databasen
                return newUser;
            }
            catch (MongoException ex) // Fångar upp eventuella fel
            {
                Console.WriteLine($"Ett fel uppstod vid registreringen av användaren: {ex.Message}");
                return null; // Returnerar null om något går fel
            }
        }

        // Metod för att kontrollera att personnumret är giltigt
        public bool IsValidPersonalNumber(string? personalNumber)
        {
            // Kontrollerar att personnumret inte är tomt
            if (string.IsNullOrEmpty(personalNumber))
            {
                return false;
            }

            // Kontrollerar att personnumret är 12 tecken långt och endast innehåller siffror
            if (!Regex.IsMatch(personalNumber, @"^\d{12}$"))
            {
                return false;
            }

            // Delar upp personnumret i år, månad och dag
            string year = personalNumber.Substring(0, 4);
            string month = personalNumber.Substring(4, 2);
            string day = personalNumber.Substring(6, 2);

            // Kontrollerar att datumet är giltigt
            if (int.TryParse(year, out int yearNum) && int.TryParse(month, out int monthNum) && int.TryParse(day, out int dayNum))
            {
                try
                {
                    DateTime date = new DateTime(yearNum, monthNum, dayNum); // Skapar ett nytt datumobjekt
                    return date <= DateTime.Today; // Returnerar true om datumet är mindre än eller lika med dagens datum
                }
                catch
                {
                    return false; // Returnerar false om datumet är ogiltigt
                }
            }
            return false; // Returnerar false vid ogiltigt personnummer
        }

        // Metod för att hämta en användare baserat på personnummer
        public async Task<User?> GetUserByPersonalNumber(string personalNumber)
        {
            try
            {
                return await usersCollection.Find(u => u.PersonalNumber == personalNumber).FirstOrDefaultAsync(); // Hämtar användaren från databasen
            }
            catch (MongoException ex) // Fångar upp eventuella fel
            {
                Console.WriteLine($"Ett fel uppstod vid hämtning av användaren: {ex.Message}");
                return null; // Returnerar null om något går fel
            }
        }
    }
}