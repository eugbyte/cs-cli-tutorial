namespace CommandLineTutorial.Models;

public record InterestInfo(string Id, decimal InterestRate, DateTime Start, DateTime? End = null) {
	public static string DateStringFormat { get; } = "yyyyMMdd";
}
