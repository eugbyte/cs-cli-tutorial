using System.ComponentModel;

namespace CommandLineTutorial.Domains.Models;

public record TransactionInfo(DateTime Date, string TransactionId, ACTION Action, decimal Amount, decimal LatestBalance) {
	public static string DateStringFormat { get; } = "yyyyMMdd";
};

public enum ACTION {
	[Description("Deposit")]
	D = 0,
	[Description("Withdraw")]
	W = 1,
}