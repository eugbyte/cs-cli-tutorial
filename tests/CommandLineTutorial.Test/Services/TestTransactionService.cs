using CommandLineTutorial.Domains.Models;
using CommandLineTutorial.Services;

namespace CommandLineTutorial.Test.Services;
public class TestTransactionService {
	private readonly string accountId = "AC001";
	private readonly decimal amount = 200;

	[Fact]
	public void TransactionService_Accept_Deposit() {
		TransactionService transactionService = new();
		DateTime date = new(year: 2024, month: 6, day: 1);
		string actionStr = "D";
		_ = Enum.TryParse(actionStr, out ACTION action);

		TransactionInfo transaction = transactionService.AddTransaction(date, accountId, action, amount);

		TransactionInfo expectedInfo = new(date, "20240601-01", action, amount, LatestBalance: amount);
		// Records have value equality
		Assert.Equal(expectedInfo, transaction);

		List<TransactionInfo> transactions = transactionService.GetTransactions(accountId);
		Assert.Single(transactions);
		Assert.Equal(expectedInfo, transactions[0]);
	}

	[Fact]
	public void Negative_Balance_Throws() {
		TransactionService transactionService = new();
		DateTime date = new(year: 2024, month: 6, day: 1);
		decimal amount = 200;
		string actionStr = "W";
		_ = Enum.TryParse(actionStr, out ACTION action);

		Assert.Throws<ArgumentException>(() => transactionService.AddTransaction(date, accountId, action, amount));
	}

	[Fact]
	public void TransactionId_Should_Increment_For_Same_Date() {
		TransactionService transactionService = new();
		DateTime date = new(year: 2024, month: 6, day: 1);
		decimal amount = 200;
		string actionStr = "D";
		_ = Enum.TryParse(actionStr, out ACTION action);

		transactionService.AddTransaction(date, accountId, action, amount);
		transactionService.AddTransaction(date, accountId, action, amount);

		List<TransactionInfo> transactions = transactionService.GetTransactions(accountId);
		Assert.Equal(2, transactions.Count);

		Assert.Equal("20240601-01", transactions[0].TransactionId);
		Assert.Equal("20240601-02", transactions[1].TransactionId);
	}

	[Fact]
	public void Latest_Balance_Should_Accumulate() {
		TransactionService transactionService = new();
		DateTime date = new(year: 2024, month: 6, day: 1);
		string actionStr = "D";
		_ = Enum.TryParse(actionStr, out ACTION action);

		transactionService.AddTransaction(date, accountId, action, amount);
		transactionService.AddTransaction(date, accountId, action, amount);

		List<TransactionInfo> transactions = transactionService.GetTransactions(accountId);
		Assert.Equal(amount * 2, transactions.Last().LatestBalance);
	}
}
