using System;

namespace BankApp
{
    // Klass för att hantera lån
    public class LoanService
    {
        // Metod för att förutsäga om ett lån kommer att bli beviljat
        public string PredictLoanApproval(float monthlyIncome, float monthlyExpenses, float loanAmount, string hasFixedIncome, string isEmployed, string hasCurrentLoan, string hasDebtIssues)
        {
            // Skapar en ny instans av ML-modellen och sätter datan till det som användaren har angett
            var input = new LoanPredictModel.ModelInput
            {
                MonthlyIncome = monthlyIncome,
                MonthlyExpenses = monthlyExpenses,
                LoanAmount = loanAmount,
                HasFixedIncome = hasFixedIncome,
                IsEmployed = isEmployed,
                HasCurrentLoan = hasCurrentLoan,
                HasDebtIssues = hasDebtIssues
            };

            // Använder ML-modellen för att förutsäga om lånet kommer att bli beviljat
            var result = LoanPredictModel.Predict(input);

            // Kontrollerar om förutsägelsen är "Ja" eller "Nej" och returnerar ett meddelande
            if (result.PredictedLabel == "Ja")
            {
                return "Det är troligt att ditt lån kommer att bli beviljat. Kontakta oss på 010-11 22 333 för att gå vidare med din låneansökan.";
            }
            else
            {
                return "Det är dessvärre inte troligt att ditt lån kommer att bli beviljat. Har du frågor? Kontakta oss på 010-11 22 333.";
            }
        }
    }
}
