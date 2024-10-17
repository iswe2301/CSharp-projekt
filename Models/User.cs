using MongoDB.Bson; // Används för att konvertera ObjectId till sträng
using MongoDB.Bson.Serialization.Attributes; // Används för att konvertera objektet till dokument

namespace BankApp
{
    // Klass för att representera en användare
    public class User
    {
        [BsonId] // Används för att identifiera objektet i databasen
        [BsonRepresentation(BsonType.ObjectId)] // Används för att konvertera strängen till ObjectId
        public string? Id { get; set; } // Id för användaren
        public string PersonalNumber { get; set; } // Personnummer för användaren
        public string FirstName { get; set; } // Förnamn
        public string LastName { get; set; } // Efternamn
        public string PasswordHash { get; private set; } // Hashat lösenord för användaren, privat sättning

        // Konstruktor för att skapa en ny användare och generera hashat lösenord
        public User(string personalNumber, string firstName, string lastName, string passwordHash)
        {
            PersonalNumber = personalNumber;
            FirstName = firstName;
            LastName = lastName;
            PasswordHash = passwordHash;
        }
    }
}