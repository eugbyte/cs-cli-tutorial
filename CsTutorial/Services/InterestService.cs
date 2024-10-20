using CsTutorial.Models;
using System.Globalization;
using System.Transactions;

namespace CsTutorial.Services;

public class InterestService {
    // Date against InterestInfo
    private readonly Dictionary<DateTime, InterestInfo> interests = new();

    public List<InterestInfo> GetInterests() {
        return interests
            .Values
            .OrderBy((v) => v.Date)
            .ToList();
    }

    public InterestInfo AddInterest(DateTime date, string ruleId, decimal interestRate) {
        if (!IsValidInterest(interestRate)) {
            throw new ArgumentException("Invalid interest rate");
        }
        interests[date] = new InterestInfo(date, ruleId, interestRate);
        return interests[date];
    }

    private static bool IsValidInterest(decimal interestRate) {
        return interestRate > 0 && interestRate < 100;
    }

    public decimal CalculateInterest(IList<TransactionInfo> transactions, DateTime start, DateTime end) {
        decimal[] interestRates = new decimal[30];
        decimal[] balances = new decimal[30];

        for (int i = 0; i < transactions.Count; i++) {
            TransactionInfo transaction = transactions[i];

            DateTime date = transaction.Date;

            if (date > start) {
                break;
            }

            decimal amount = transaction.Amount;
            if (transaction.Action == "W") {
                balances[0] -= amount;
            } else {
                balances[0] += amount;
            }
        }

        List<InterestInfo> interestInfos = interests
            .Values
            .OrderBy(v => v.Date)
            .ToList();

        for (int i = 0; i < interestInfos.Count; i++) { 
            InterestInfo interestInfo = interestInfos[i];
            DateTime date = interestInfo.Date;

            if (date <= start) {
                interestRates[0] = interestInfo.InterestRate;
            }
        }

        return 0;
    }
}
