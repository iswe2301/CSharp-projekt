using Microsoft.AspNetCore.Identity; // Används för att hasha lösenord

namespace BankApp
{
    // Klass för att hantera autentisering och inloggning av användare
    public class AuthService
    {
        private readonly PasswordHasher<User> passwordHasher = new PasswordHasher<User>(); // Skapar en ny instans av PasswordHasher

        // Metod för att autentisera användaren vid inloggning
        public bool Authenticate(User user, string inputPassword)
        {
            try
            {
                var password = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, inputPassword); // Jämför användarens inmatade lösen med det hashade lösenordet
                return password == PasswordVerificationResult.Success; // Returnerar true om lösenordet är korrekt
            }
            catch (Exception ex) // Fångar upp eventuella fel
            {
                Console.WriteLine($"Ett fel uppstod vid autentisering: {ex.Message}");
                return false; // Returnerar false om något går fel vid autentiseringen
            }
        }
    }
}