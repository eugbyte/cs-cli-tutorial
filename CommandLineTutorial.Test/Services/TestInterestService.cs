using CommandLineTutorial.Models;
using CommandLineTutorial.Services;

namespace CommandLineTutorial.Test.Services;

public class TestInterestService {
	private readonly string accountId = "AC001";

	[Fact]
	public void Add_Interest() {
		InterestService interestService = new();

		DateTime date = new(year: 2024, month: 6, day: 1);
		string ruleId = "RULE01";
		interestService.AddInterest(date, ruleId, 10);
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
	public void Calculated_Interest_1() {
		InterestService interestService = new();

		DateTime date1 = new(year: 2024, month: 6, day: 1);
		interestService.AddInterest(date1, "RULE01", 10);

		DateTime date2 = new(year: 2024, month: 6, day: 15);
		interestService.AddInterest(date2, "RULE02", 20);

		List<TransactionInfo> transactionInfos = [
			new(Date: date1, TransactionId: "1", Action: ACTION.D, Amount: 1000, LatestBalance: 1000)
		];

		DateTime date3 = new(year: 2024, month: 6, day: 30);
		decimal interest = interestService.CalculateInterest(transactionInfos, start: date1, end: date3);

		interest = Math.Round(interest, 2);
		Assert.Equal(12.32m, interest);
	}

	[Fact]
	public void Calculated_Interest_2() {
		InterestService interestService = new();

		DateTime date1 = new(year: 2024, month: 6, day: 1);
		interestService.AddInterest(date1, "RULE01", 10);

		List<TransactionInfo> transactionInfos = [
			new(Date: date1, TransactionId: "1", Action: ACTION.D, Amount: 1000, LatestBalance: 1000),
			new(Date: new(year: 2024, month: 6, day: 15), TransactionId: "2", Action: ACTION.W, Amount: 500, LatestBalance: 500)
		];

		DateTime date3 = new(year: 2024, month: 6, day: 30);
		decimal interest = interestService.CalculateInterest(transactionInfos, start: date1, end: date3);

		interest = Math.Round(interest, 2);
		Assert.Equal(12.32m, interest);
	}

	[Fact]
	public void Calculated_Interest_3() {
		InterestService interestService = new();

		DateTime date1 = new(year: 2024, month: 6, day: 1);
		interestService.AddInterest(date1, "RULE01", 10);

		DateTime date2 = new(year: 2024, month: 6, day: 10);
		interestService.AddInterest(date2, "RULE02", 20);

		List<TransactionInfo> transactionInfos = [
			new(Date: new(year: 2024, month: 6, day: 1), TransactionId: "1", Action: ACTION.D, Amount: 1000, LatestBalance: 1000),
			new(Date: new(year: 2024, month: 6, day: 20), TransactionId: "2", Action: ACTION.W, Amount: 500, LatestBalance: 1000)
		];

		DateTime date3 = new(year: 2024, month: 6, day: 30);
		decimal interest = interestService.CalculateInterest(transactionInfos, start: date1, end: date3);

		interest = Math.Round(interest, 2);
		Assert.Equal(12.32m, interest);
	}
}
