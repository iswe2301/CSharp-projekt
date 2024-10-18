namespace BankApp
{
    // Klass för att hantera input från användaren och validera den
    public static class InputValidation
    {
        // Metod för att hämta ett giltigt personnummer
        public static string? GetValidPersonalNumber(UserService userService)
        {
            while (true)
            {
                Console.Write("Ange ditt personnummer (ÅÅÅÅMMDDXXXX) eller skriv 'X' för att avbryta: ");
                string personalNumber = Console.ReadLine()?.Trim() ?? ""; // Läser in och trimmar input, om input är null sätts den till en tom sträng

                // Kontrollerar om användaren vill avbryta
                if (personalNumber.ToUpper() == "X") return null; // Returnerar null för att avbryta

                if (userService.IsValidPersonalNumber(personalNumber)) // Validerar personnumret genom att anropa metoden i UserService
                    return personalNumber; // Returnerar personnumret om det är giltigt
                Console.WriteLine("Fel: Ange giltigt personnummer.");
            }
        }

        // Metod för att hantera giltig input för förnamn/efternamn, får ej vara tomt
        public static string? GetValidInput(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt} eller skriv 'X' för att avbryta: "); // Visar prompten
                string input = Console.ReadLine()?.Trim() ?? ""; // Läser in och trimmar input, om input är null sätts den till en tom sträng

                // Kontrollerar om användaren vill avbryta
                if (input.ToUpper() == "X") return null; // Returnerar null för att avbryta

                // Kontrollerar att input inte är tom
                if (!string.IsNullOrWhiteSpace(input))
                    return input; // Returnerar giltig input
                Console.WriteLine("Fel: Fältet får inte vara tomt."); // Felmeddelande om input är tom
            }
        }

        // Metod för att kontrollera giltigt lösenord, får ej vara tomt och minst 5 tecken långt
        public static string? GetValidPassword(bool checkLength)
        {
            while (true)
            {
                Console.Write("Ange ditt lösenord eller skriv 'X' för att avbryta: ");

                string? password = HidePassword(); // Anropar metod för att dölja lösenordet

                // Kontrollerar om användaren vill avbryta
                if (password == null) return null; // Returnerar null för att avbryta

                // Kontrollerar att lösenordet inte är tomt och att det är minst 5 tecken långt (om checkLength är true)
                if (!checkLength || password.Length >= 5)
                    return password; // Returnerar giltigt lösenord
                Console.WriteLine("Fel: Lösenordet måste vara minst 5 tecken långt."); // Felmeddelande om lösenordet är för kort
            }
        }

        // Metod för att dölja lösenord som skrivs in
        public static string? HidePassword()
        {
            var password = string.Empty; // Tom sträng för att lagra lösenordet

            // Loop för att läsa in lösenordet tecken för tecken
            while (true)
            {
                var keyInfo = Console.ReadKey(intercept: true); // Läser in tangenttryck utan att visa det

                // Kontrollerar om användaren trycker på Enter
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    // Kontrollerar om användaren vill avbryta
                    if (password.ToUpper() == "X")
                    {
                        return null; // Returnerar null för att avbryta
                    }
                    break; // Avslutar loopen när användaren trycker Enter
                }

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
        public static decimal? GetValidAmount(string prompt, decimal minimumAmount)
        {
            decimal amount; // Variabel för att lagra beloppet

            // Loopar tills användaren matat in ett giltigt belopp
            while (true)
            {
                Console.Write($"{prompt} eller skriv 'X' för att avbryta: "); // Skriver ut prompten
                string? amountInput = Console.ReadLine();

                // Kontrollerar om användaren vill avbryta
                if (amountInput?.ToUpper() == "X") return null; // Returnerar null för att avbryta

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

        // Metod för att hantera gitlig input av kontotyp, får ej vara tomt
        public static string? GetAccountType()
        {
            string? accountType = null; // Variabel för att lagra kontotypen

            // Loopar tills användaren har angett en giltig kontotyp (inte tom)
            while (string.IsNullOrWhiteSpace(accountType))
            {
                Console.Write("\nAnge kontotyp (t.ex. Sparkonto, Lönekonto) eller skriv 'X' för att avbryta: ");

                accountType = Console.ReadLine();

                // Kontrollerar om användaren vill avbryta
                if (accountType?.ToUpper() == "X") return null; // Returnerar null för att avbryta

                // Kontrllerar om kontotypen är tom
                if (string.IsNullOrWhiteSpace(accountType))
                {
                    Console.WriteLine("Fel: Kontotyp kan inte vara tom. Försök igen."); // Skriver ut felmeddelande
                }
            }
            return accountType; // Returnerar kontotypen
        }

        // Metod för att hämta giltigt startbelopp
        public static decimal? GetInitialBalance()
        {
            decimal initialBalance = 0; // Variabel för att lagra startbeloppet, sätts initalt till 0

            // Loopar tills användaren har angett ett giltigt startbelopp (minst 100 kr)
            while (initialBalance < 100)
            {
                Console.Write("Ange startbelopp (minst 100 kr) eller skriv 'X' för att avbryta: ");

                string? balanceInput = Console.ReadLine();

                // Kontrollerar om användaren vill avbryta
                if (balanceInput?.ToUpper() == "X") return null; // Returnerar null för att avbryta

                // Kontrollerar att beloppet är giltigt och minst 100 kr
                if (string.IsNullOrWhiteSpace(balanceInput) || !decimal.TryParse(balanceInput, out initialBalance) || initialBalance < 100)
                {
                    Console.WriteLine("Fel: Startbeloppet måste vara minst 100 kr. Försök igen."); // Felmeddelande om beloppet är ogiltigt
                }
            }
            return initialBalance; // Returnerar startbeloppet
        }

        // Metod för att välja ett specifikt konto från en lista
        public static Account? SelectAccount(List<Account> accounts, string prompt)
        {
            Console.WriteLine("\nMina konton:");

            // Loopar igenom användarens konton och skriver ut dem
            for (int i = 0; i < accounts.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {accounts[i].AccountType} - {accounts[i].AccountNumber} (Saldo: {accounts[i].Balance:C})");
            }

            int selectedAccountIndex; // Variabel för att lagra det valda kontots index

            // Loopar tills användaren valt ett giltigt konto
            while (true)
            {
                Console.Write(prompt); // Skriver ut meddelandet som skickats in
                string? selectedAccountIndexInput = Console.ReadLine(); // Läser in användarens val

                // Kontrollerar om användaren vill avbryta
                if (selectedAccountIndexInput?.ToUpper() == "X") return null; // Returnerar null om användaren väljer att avbryta

                // Kontrollerar att användaren valt ett giltigt konto
                if (int.TryParse(selectedAccountIndexInput, out selectedAccountIndex) && selectedAccountIndex >= 1 && selectedAccountIndex <= accounts.Count)
                {
                    return accounts[selectedAccountIndex - 1]; // Returnerar det valda kontot (-1 pga index)
                }
                else
                {
                    Console.WriteLine("Fel: Ogiltigt val. Försök igen."); // Skriver ut felmeddelande om valet inte är giltigt
                }
            }
        }

        // Metod för att validera och hämta ja/nej-input
        public static string? GetYesOrNoInput(string prompt)
        {
            // Loopar tills användaren har angett "Ja" eller "Nej"
            while (true)
            {
                Console.Write(prompt); // Skriver ut prompten
                string? input = Console.ReadLine()?.Trim().ToUpper(); // Läser in och trimmar input, konverterar till versaler

                // Kontrollerar om användaren vill avbryta
                if (input == "X")
                {
                    return null; // Returnerar null för att avbryta
                }
                // Kontrollerar om input är ja eller nej
                else if (input == "JA" || input == "NEJ")
                {
                    return input; // Returnerar giltig input
                }
                else
                {
                    Console.WriteLine("Fel: Ange 'Ja' eller 'Nej'. Försök igen."); // Felmeddelande om input inte är ja eller nej
                }
            }
        }

        // Metod för att validera och hämta giltig input för belopp i låneansökan
        public static float GetValidMoneyInput(string prompt, float minValue)
        {
            // Loopar tills användaren har angett ett giltigt belopp som är minst minValue
            while (true)
            {
                Console.Write(prompt); // Skriver ut prompten
                string? input = Console.ReadLine()?.Trim(); // Läser in och trimmar input

                // Kontrollerar om användaren vill avbryta
                if (input?.ToUpper() == "X")
                {
                    return -1; // Returnerar -1 för att avbryta
                }
                // Kontrollerar att input är ett giltigt belopp som är minst minValue
                if (float.TryParse(input, out float result) && result >= minValue)
                {
                    return result; // Returnerar det giltiga beloppet
                }
                else
                {
                    Console.WriteLine($"Fel: Ange ett giltigt belopp som är minst {minValue} kr. Försök igen."); // Felmeddelande om beloppet är ogiltigt
                }
            }
        }
    }
}
