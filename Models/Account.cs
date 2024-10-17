using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BankApp
{
    // Klass för att representera ett konto
    public class Account
    {
        [BsonId] // Används för att identifiera objektet i databasen
        [BsonRepresentation(BsonType.ObjectId)] // Används för att konvertera strängen till ObjectId
        public string? Id { get; set; } // Id för kontot
        public string? AccountNumber { get; set; } // Kontonummer
        public string? OwnerPersonalNumber { get; set; } // Personnummer för kontots ägare
        public string? AccountType { get; set; } // Typ av konto
        public decimal Balance { get; set; } // Kontots saldo
    }
}