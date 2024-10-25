using CommandLineTutorial.Domains.Interfaces;
using CommandLineTutorial.Domains.Models;
using CommandLineTutorial.Services.Loggers;
using System.Globalization;

namespace CommandLineTutorial.Handlers;

public class MainHandler(IInterestService interestService, ITransactionService transactionService) {
	public void Run() {
		bool shouldRun = true;

		while (shouldRun) {
			Console.WriteLine(@"
Welcome to Awesome Bank! What would you like to do?
[T] Input transactions 
[I] Define interest rules
[P] Print statement
[Q] Quit");

			string input = (Console.ReadLine() ?? "").ToUpper();
			switch (input) {
				case "T":
					ProcessTransaction();
					break;
				case "Q":
					shouldRun = false;
					Console.WriteLine("Thank you for banking with Awesome Bank.\r\nHave a nice day!");
					break;
				case "I":
					ProcessInterest();
					break;
				case "P":
					ProcessSummary();
					break;
				default:
					continue;
			}

		}
	}

	private void ProcessTransaction() {
		Console.WriteLine("Please enter transaction details in <Date> <Account> <Type> <Amount> format");
		string input = Console.ReadLine() ?? "";

		string[] details = input.Split(" ");

		if (details.Length != 4) {
			throw new ArgumentException("Wrong input format");
		}

		string dateStr = details[0];
		string accountId = details[1];
		string actionStr = details[2];
		string amountStr = details[3];
		decimal amount = decimal.Parse(amountStr);

		DateTime date = DateTime.Now;
		if (!DateTime.TryParseExact(dateStr, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date)) {
			throw new ArgumentException("Wrong date format");
		}

		_ = Enum.TryParse(actionStr, out ACTION action);

		transactionService.AddTransaction(date, accountId, action, amount);
		IList<TransactionInfo> transactionInfos = transactionService.GetTransactions(accountId);
		TransactionLogger.Log(accountId, transactionInfos);
	}

	private void ProcessInterest() {
		Console.WriteLine("Please enter interest rules details in <Date> <RuleId> <Rate in %> format \r\n(or enter blank to go back to main menu):");
		string input = Console.ReadLine() ?? "";

		string[] details = input.Split(" ");

		string dateStr = details[0];
		string ruleId = details[1];
		string interestRateStr = details[2];
		decimal interestRate = decimal.Parse(interestRateStr);
		if (!DateTime.TryParseExact(dateStr, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date)) {
			throw new ArgumentException("Wrong date format");
		}

		interestService.AddInterest(date, ruleId, interestRate);
		IList<InterestInfo> interests = interestService.GetInterests();
		InterestLogger.Log(interests);
	}

	private void ProcessSummary() {
		Console.WriteLine("Please enter account and month to generate the statement <Account> <Year><Month>\r\n(or enter blank to go back to main menu):");
		string input = Console.ReadLine() ?? "";

		string[] details = input.Split(" ");

		string accountId = details[0];
		string dateStr = details[1];
		int year = int.Parse(dateStr[..4]);
		int month = int.Parse(dateStr.Substring(4, 2));

		List<TransactionInfo> transactionInfos = transactionService.GetTransactions(accountId)
			.Where((info) => info.Date.Year == year && info.Date.Month == month)
			.ToList();
		TransactionLogger.Log(accountId, transactionInfos);

		DateTime start = new(year: year, month: month, day: 1);
		DateTime end = new(year: year, month: month, day: 30);

		decimal interest = interestService.CalculateInterest(transactionInfos, start, end);
		BalanceLoger.Log(transactionInfos, end, interest);
	}
}
