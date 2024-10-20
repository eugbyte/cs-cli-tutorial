using CsTutorial.Models;
using System.Globalization;
using System.Transactions;

namespace CsTutorial.Services;

public class InterestService {
    // composite id (dateStr + ruleId) against InterestInfo
    private readonly Dictionary<string, InterestInfo> interests = new();

    public List<InterestInfo> GetInterests() {
        return interests
            .Values
            .OrderBy((v) => v.DateStr)
            .ToList();
    }

    public InterestInfo AddInterest(string dateStr, string ruleId, decimal interestRate) {
        if (!IsValidInterest(interestRate)) {
            throw new ArgumentException("Invalid interest rate");
        }
        interests[dateStr] = new InterestInfo(dateStr, ruleId, interestRate);
        return interests[dateStr];
    }

    private static bool IsValidInterest(decimal interestRate) {
        return interestRate > 0 && interestRate < 100;
    }

    public decimal CalculateInterest(IList<TransactionInfo> transactions, DateTime start, DateTime end) {
        decimal[] interestRates = new decimal[30];
        decimal[] balances = new decimal[30];

        for (int i = 0; i < transactions.Count; i++) {
            TransactionInfo transaction = transactions[i];

            DateTime date;
            DateTime.TryParseExact(transaction.DateStr, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

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
            .OrderBy(v => v.DateStr)
            .ToList();

        for (int i = 0; i < interestInfos.Count; i++) { 
            InterestInfo interestInfo = interestInfos[i];
            DateTime date;
            DateTime.TryParseExact(interestInfo.DateStr, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

            if (date <= start) {
                interestRates[0] = interestInfo.InterestRate;
            }
        }

        return 0;
    }
}
