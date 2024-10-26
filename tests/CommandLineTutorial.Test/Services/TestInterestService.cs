using CommandLineTutorial.Domains.Models;
using CommandLineTutorial.Services;

namespace CommandLineTutorial.Test.Services;

public class TestInterestService {
	[Fact]
	public void Add_Interest() {
		InterestService interestService = new();

		DateTime date = new(year: 2024, month: 6, day: 1);
		string ruleId = "RULE01";
		interestService.AddInterest(date, ruleId, 10);
	}

	[Fact]
	public void Invalid_Interest_Throw() {
		InterestService interestService = new();

		DateTime date = new(year: 2024, month: 6, day: 1);
		string ruleId = "RULE01";
		Assert.Throws<ArgumentException>(() => interestService.AddInterest(date, ruleId, 0));
	}

	[Fact]
	public void Unique_Interest_On_Same_Date() {
		InterestService interestService = new();

		DateTime date = new(year: 2024, month: 6, day: 1);
		interestService.AddInterest(date, "RULE01", 10);

		date = date.AddDays(1);
		interestService.AddInterest(date, "RULE02", 10);
		Assert.Equal(2, interestService.GetInterests().Count);

		interestService.AddInterest(date, "RULE03", 10);
		Assert.Equal(2, interestService.GetInterests().Count);

		List<InterestInfo> interests = interestService.GetInterests();
		Assert.Equal("RULE03", interests.Last().Id);
	}

	[Fact]
	public void Calculated_Interest_Two_IR_Overlaps() {
		InterestService interestService = new();

		DateTime start = new(year: 2024, month: 6, day: 1);
		interestService.AddInterest(start, "RULE01", 10);

		DateTime date2 = new(year: 2024, month: 6, day: 16);
		interestService.AddInterest(date2, "RULE02", 20);

		List<TransactionInfo> transactionInfos = [
			new(Date: start, TransactionId: "1", Action: ACTION.D, Amount: 1000, LatestBalance: 1000)
		];

		DateTime end = new(year: 2024, month: 6, day: 30);
		decimal interest = interestService.CalculateInterest(transactionInfos, start, end);

		interest = Math.Round(interest, 2);
		Assert.Equal(12.33m, interest);
	}

	[Fact]
	public void Calculated_Interest_Two_Balance_Overlaps() {
		InterestService interestService = new();

		DateTime start = new(year: 2024, month: 6, day: 1);
		interestService.AddInterest(start, "RULE01", 10);

		List<TransactionInfo> transactionInfos = [
			new(Date: start, TransactionId: "1", Action: ACTION.D, Amount: 1000, LatestBalance: 1000),
			new(Date: new(year: 2024, month: 6, day: 16), TransactionId: "2", Action: ACTION.W, Amount: 500, LatestBalance: 500)
		];

		DateTime end = new(year: 2024, month: 6, day: 30);
		decimal interest = interestService.CalculateInterest(transactionInfos, start, end);

		interest = Math.Round(interest, 2);
		Assert.Equal(6.16m, interest);
	}

	[Fact]
	public void Calculated_Interest_Three_Overlaps() {
		InterestService interestService = new();

		DateTime start = new(year: 2024, month: 6, day: 1);
		interestService.AddInterest(start, "RULE01", 10);

		DateTime date2 = new(year: 2024, month: 6, day: 11);
		interestService.AddInterest(date2, "RULE02", 20);

		List<TransactionInfo> transactionInfos = [
			new(Date: start, TransactionId: "1", Action: ACTION.D, Amount: 1000, LatestBalance: 1000),
			new(Date: new(year: 2024, month: 6, day: 16), TransactionId: "2", Action: ACTION.W, Amount: 500, LatestBalance: 500)
		];

		DateTime end = new(year: 2024, month: 6, day: 30);
		decimal interest = interestService.CalculateInterest(transactionInfos, start, end);

		interest = Math.Round(interest, 2);
		Assert.Equal(9.59m, interest);
	}
}
