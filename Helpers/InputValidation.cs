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

        // Metod för att hantera giltig input för förnamn/efternamn, får ej vara tomt
        public static string GetValidInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt); // Visar prompten
                string input = Console.ReadLine()?.Trim() ?? ""; // Läser in och trimmar input, om input är null sätts den till en tom sträng
                // Kontrollerar att input inte är tom
                if (!string.IsNullOrWhiteSpace(input))
                    return input; // Returnerar giltig input
                Console.WriteLine("Fel: Fältet får inte vara tomt."); // Felmeddelande om input är tom
            }
        }
    }
}
