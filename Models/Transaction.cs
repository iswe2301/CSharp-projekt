using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BankApp
{
    // Klass för att representera en transaktion
    public class Transaction
    {
        [BsonId] // Används för att identifiera objektet i databasen
        [BsonRepresentation(BsonType.ObjectId)] // Används för att konvertera strängen till ObjectId
        public string? Id { get; set; } // Id för transaktionen
        public string? AccountNumber { get; set; } // Kontonummer kopplat till transaktionen
        public string? TransactionType { get; set; } // Typ av transaktion: Insättning/Uttag
        public decimal Amount { get; set; } // Belopp för transaktionen
        public DateTime Date { get; set; } // Datum för transaktionen

        // Konstruktor för att skapa en transaktion
        public Transaction(string accountNumber, string transactionType, decimal amount)
        {
            AccountNumber = accountNumber;
            TransactionType = transactionType;
            Amount = amount;
            Date = DateTime.Now; // Sätter transaktionsdatum till aktuellt datum/tid
        }
    }
}
