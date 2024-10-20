using CsTutorial.Models;

namespace CsTutorial.Services;

public class InterestLogger {
    public static string Log(IList<InterestInfo> interestInfos)
    {
        const string fmt = "|{0, -10} | {1, -15} | {2, -10}|";
        string header = string.Format(fmt, "Date", "RuleId", "Rate (%)");
        List<string> rows = interestInfos
            .Select(info => string.Format(fmt, info.DateStr, info.Id, info.InterestRate))
            .ToList();
        return string.Join("\n", [header, ..rows]);
    }
}
