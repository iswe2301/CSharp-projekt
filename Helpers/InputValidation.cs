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

        // Metod för att kontrollera giltigt lösenord, får ej vara tomt och minst 5 tecken långt
        public static string GetValidPassword(bool checkLength)
        {
            while (true)
            {
                Console.Write("Ange ditt lösenord: ");
                string password = HidePassword(); // Anropar metod för att dölja lösenordet
                // Kontrollerar att lösenordet inte är tomt och att det är minst 5 tecken långt (om checkLength är true)
                if (!checkLength || password.Length >= 5)
                    return password; // Returnerar giltigt lösenord
                Console.WriteLine("Fel: Lösenordet måste vara minst 5 tecken långt."); // Felmeddelande om lösenordet är för kort
            }
        }

        // Metod för att dölja lösenord som skrivs in
        public static string HidePassword()
        {
            var password = string.Empty; // Tom sträng för att lagra lösenordet

            // Loop för att läsa in lösenordet tecken för tecken
            while (true)
            {
                var keyInfo = Console.ReadKey(intercept: true); // Läser in tangenttryck utan att visa det
                if (keyInfo.Key == ConsoleKey.Enter)
                    break; // Avslutar loopen när användaren trycker Enter

                // Kontrollerar om användaren trycker på backspace och att lösenordet inte är tomt
                if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1); // Tar bort sista tecknet från lösenordet
                    Console.Write("\b \b"); // Flyttar markören bakåt, skriver över tecknet med ett mellanslag och flyttar markören bakåt igen
                }
                // Kontrollerar att tecknet inte är ett kontrolltecken
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    password += keyInfo.KeyChar; // Lägger till tecknet i lösenordet
                    Console.Write("*"); // Visar en * istället för tecknet
                }
            }
            Console.WriteLine(); // Skapar en ny rad efter lösenordet skrivits in
            return password; // Returnerar det dolda lösenordet
        }

        // Metod för att validera och hämta ett giltigt belopp
        public static decimal GetValidAmount(string prompt, decimal minimumAmount)
        {
            decimal amount; // Variabel för att lagra beloppet

            // Loopar tills användaren matat in ett giltigt belopp
            while (true)
            {
                Console.Write(prompt); // Skriver ut prompten
                string? amountInput = Console.ReadLine();

                // Kontrollerar att beloppet är giltigt och minst minimunbeloppet som skickats in
                if (decimal.TryParse(amountInput, out amount) && amount >= minimumAmount)
                {
                    return amount; // Returnerar det giltiga beloppet
                }
                else
                {
                    Console.WriteLine($"Fel: Beloppet måste vara minst {minimumAmount:C}. Försök igen."); // Felmeddelande om beloppet är ogiltigt
                }
            }
        }
    }
}
