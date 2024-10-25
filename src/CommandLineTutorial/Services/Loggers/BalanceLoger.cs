using CommandLineTutorial.Models;

namespace CommandLineTutorial.Services.Loggers;
public class BalanceLoger {
	public static void Log(IList<TransactionInfo> transactionInfos, DateTime date, decimal interest) {
		const string fmt = "|{0, -10} | {1, -15} | {2, -10} | {3, -10} | {4, -10}|";

		string header = string.Format(fmt, "Date", "Txn Id", "Type", "Amount", "Balance");
		List<string> rows = transactionInfos
			.Select(info => {
				string dateStr = info.Date.ToString(TransactionInfo.DateStringFormat);
				return string.Format(fmt, dateStr, info.TransactionId, info.Action, info.Amount, info.LatestBalance);
			})
			.ToList();

		string total = (transactionInfos.Last().LatestBalance + interest).ToString("0.00");
		string interestRow = string.Format(fmt, date.ToString(InterestInfo.DateStringFormat), "", "I", interest.ToString("0.00"), total);

		Console.WriteLine(string.Join("\n", [header, .. rows, interestRow]));
	}
}
