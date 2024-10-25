using CommandLineTutorial.Domains.Models;

namespace CommandLineTutorial.Services.Loggers;

public class InterestLogger {
	public static void Log(IList<InterestInfo> interestInfos) {
		const string fmt = "|{0, -10} | {1, -15} | {2, -10}|";
		string header = string.Format(fmt, "Date", "RuleId", "Rate (%)");
		List<string> rows = interestInfos
			.Select(info => {
				string dateStr = info.Start.ToString(InterestInfo.DateStringFormat);
				return string.Format(fmt, dateStr, info.Id, info.InterestRate);
			})
			.ToList();
		Console.WriteLine($"Interest rules: \n{string.Join("\n", [header, .. rows])}");
	}
}
