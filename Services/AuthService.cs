using Microsoft.AspNetCore.Identity; // Används för att hasha lösenord

namespace BankApp
{
    // Klass för att hantera autentisering och inloggning av användare
    public class AuthService
    {
        // Enum för att representera olika inloggningsresultat
        public enum LoginStatus
        {
            UserNotFound, // Användaren finns inte
            WrongPassword, // Fel lösenord
            Success // Inloggning lyckades
        }

        private readonly PasswordHasher<User> passwordHasher = new PasswordHasher<User>(); // Skapar en ny instans av PasswordHasher
        private readonly UserService userService; // Använder en instans av UserService för att hämta användare

        // Konstruktor som tar emot en instans av UserService för att hämta användare
        public AuthService(UserService userService)
        {
            this.userService = userService;
        }

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

        // Metod för att logga in befintlig användare
        public (LoginStatus, User?) LoginUser(string personalNumber, string password)
        {
            try
            {
                var user = userService.GetUserByPersonalNumber(personalNumber); // Hämtar användaren via UserService

                // Kontrollerar om användaren finns och om lösenordet är korrekt
                if (user == null)
                {
                    return (LoginStatus.UserNotFound, null); // Om användaren inte finns, returneras null och statusen UserNotFound
                }

                // Kontrollerar om lösenordet är fel
                if (!Authenticate(user, password))
                {
                    return (LoginStatus.WrongPassword, null); // Om lösenordet är fel, returneras null och statusen WrongPassword
                }

                return (LoginStatus.Success, user); // Returnerar användaren och success-status om inloggningen lyckades
            }
            catch (Exception ex) // Fångar upp eventuella fel
            {
                Console.WriteLine($"Ett fel uppstod vid inloggningen: {ex.Message}");
                return (LoginStatus.UserNotFound, null); // Returnerar felstatus om något går fel
            }
        }
    }
}