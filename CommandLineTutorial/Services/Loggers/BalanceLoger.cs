using CommandLineTutorial.Models;

namespace CommandLineTutorial.Services.Loggers;
public class BalanceLoger {
	public static string Log(IList<TransactionInfo> transactionInfos, string dateStr, decimal interest) {
		const string fmt = "|{0, -10} | {1, -15} | {2, -10} | {3, -10} | {4, -10}|";

		string header = string.Format(fmt, "Date", "Txn Id", "Type", "Amount", "Balance");
		List<string> rows = transactionInfos
			.Select(info => {
				string dateStr = info.Date.ToString(InterestInfo.DateStringFormat);
				return string.Format(fmt, dateStr, info.Id, info.Action, info.Amount, info.LatestBalance);
			})
			.ToList();
		string interestRow = string.Format(fmt, "Date", "", "I", interest, transactionInfos.Last().LatestBalance + interest);
		return string.Join("\n", [header, .. rows, interestRow]);
	}
}
