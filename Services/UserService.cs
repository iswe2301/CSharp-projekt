using MongoDB.Driver;

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
        public User? RegisterNewUser(string personalNumber, string firstName, string lastName, string password)
        {
            try
            {
                // Kontrollerar om användaren redan finns i databasen
                var existingUser = usersCollection.Find(u => u.PersonalNumber == personalNumber).FirstOrDefault();
                if (existingUser != null)
                {
                    return null; // Returnerar null om användaren redan finns
                }

                var newUser = new User(personalNumber, firstName, lastName, password); // Skapar en ny användare
                usersCollection.InsertOne(newUser); // Lägger till användaren i databasen
                return newUser;
            }
            catch (MongoException ex) // Fångar upp eventuella fel
            {
                Console.WriteLine($"Ett fel uppstod vid registreringen av användaren: {ex.Message}");
                return null; // Returnerar null om något går fel
            }
        }
    }
}