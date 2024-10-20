using CsTutorial.Models;

namespace CsTutorial.Services;

public class InterestService {
    // composite id (dateStr + ruleId) against InterestInfo
    private readonly Dictionary<string, InterestInfo> interests = new();

    public List<InterestInfo> GetInterests() {
        return [.. interests.Values];
    }

    public InterestInfo AddInterest(string dateStr, string ruleId, decimal interestRate) {
        if (!IsValidInterest(interestRate)) {
            throw new ArgumentException("Invalid interest rate");
        }
        string id = CreateCompositeId(dateStr, ruleId);
        interests[id] = new InterestInfo(dateStr, ruleId, interestRate);
        return interests[id];
    }

    private static string CreateCompositeId(string dateStr, string ruleId) {
        return $"{dateStr}-{ruleId}";
    }

    private static bool IsValidInterest(decimal interestRate) {
        return interestRate > 0 && interestRate < 100;
    }
}
