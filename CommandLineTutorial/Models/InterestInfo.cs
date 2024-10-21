namespace CommandLineTutorial.Models;

public record InterestInfo(string Id, decimal InterestRate, DateTime Start, DateTime End) {
	public static string DateStringFormat { get; } = "yyyyMMdd";
}
