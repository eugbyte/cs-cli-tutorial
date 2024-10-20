using CsTutorial.Models;

namespace CsTutorial.Services;

public class InterestLogger {
    public static string Log(IList<InterestInfo> interestInfos)
    {
        string header = string.Format("|{0, -10} | {1, -15} | {2, -10}|", "Date", "RuleId", "Rate (%)");
        List<string> rows = interestInfos
            .Select(info => string.Format("|{0, -10} | {1, -15} | {2, -10}|", info.DateStr, info.Id, info.InterestRate))
            .ToList();
        return string.Join("\n", [header, ..rows]);
    }
}
