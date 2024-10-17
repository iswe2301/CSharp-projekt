namespace BankApp
{
    // Klass för att hantera input från användaren och validera den
    public static class InputValidation
    {
        // Metod för att hämta ett giltigt personnummer
        public static string GetValidPersonalNumber(UserService userService)
        {
            while (true)
            {
                Console.Write("Ange ditt personnummer (ÅÅÅÅMMDDXXXX): ");
                string personalNumber = Console.ReadLine() ?? ""; // Läser in personnumret
                if (userService.IsValidPersonalNumber(personalNumber)) // Validerar personnumret genom att anropa metoden i UserService
                    return personalNumber; // Returnerar personnumret om det är giltigt
                Console.WriteLine("Fel: Ange giltigt personnummer.");
            }
        }
    }
}
