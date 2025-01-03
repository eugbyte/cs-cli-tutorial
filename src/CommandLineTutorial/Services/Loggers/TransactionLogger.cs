﻿using CommandLineTutorial.Domains.Models;

namespace CommandLineTutorial.Services.Loggers;

public class TransactionLogger {
	public static void Log(string accountId, IList<TransactionInfo> transactionInfos) {
		const string fmt = "|{0, -10} | {1, -15} | {2, -10} | {3, -10}|";

		string header = string.Format(fmt, "Date", "Txn Id", "Type", "Amount");
		List<string> rows = transactionInfos
			.Select(info => {
				string dateStr = info.Date.ToString(InterestInfo.DateStringFormat);
				return string.Format(fmt, dateStr, info.TransactionId, info.Action, info.Amount);
			})
			.ToList();

		Console.WriteLine($"Account: {accountId}\n{string.Join("\n", [header, .. rows])}");
	}
}
