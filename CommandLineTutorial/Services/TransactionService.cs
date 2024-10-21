using CommandLineTutorial.Models;

namespace CommandLineTutorial.Services;

public class TransactionService {
	// date string against count
	private readonly Dictionary<string, Dictionary<string, int>> transactionCounts = new();
	// accountId against a list of transactions
	private readonly Dictionary<string, List<TransactionInfo>> transactions = new();

	public List<TransactionInfo> GetTransactions(string accountId) {
		return transactions[accountId]
			.OrderBy((v) => v.Date)
			.ToList();
	}

	public TransactionInfo AddTransaction(DateTime date, string accountId, ACTION action, decimal amount) {
		if (action == ACTION.W) {
			amount *= -1;
		}

		if (!transactions.ContainsKey(accountId)) {
			transactions[accountId] = [];
		}
		decimal prevBalance = transactions[accountId].Count == 0 ? 0 : transactions[accountId].Last().LatestBalance;
		if (prevBalance + amount < 0) {
			throw new ArgumentException("Negative balance not allowed");
		}

		string transactionId = CreateTransactionId(accountId, date);
		TransactionInfo transaction = new(date, transactionId, action, amount, prevBalance + amount);

		transactions[accountId].Add(transaction);
		return transaction;
	}

	private string CreateTransactionId(string accountId, DateTime date) {
		string dateStr = date.ToString(TransactionInfo.DateStringFormat);
		if (!transactionCounts.ContainsKey(accountId)) {
			transactionCounts[accountId] = new();
		}
		Dictionary<string, int> userTransactionCounts = transactionCounts[accountId];

		if (!userTransactionCounts.ContainsKey(dateStr)) {
			userTransactionCounts[dateStr] = 0;
		}
		userTransactionCounts[dateStr] += 1;
		int count = userTransactionCounts[dateStr];
		int padLen = Math.Max(2, count.ToString().Length);
		string countStr = count.ToString().PadLeft(padLen, '0');
		return $"{dateStr}-{countStr}";
	}
}
