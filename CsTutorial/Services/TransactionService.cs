using CsTutorial.Models;

namespace CsTutorial.Services;

public class TransactionService {
    // accountId against account balance
    private readonly Dictionary<string, decimal> balances = new();
    // date string against count
    private readonly Dictionary<string, Dictionary<string, int>> transactionCounts = new();
    // accountId against a list of transactions
    private readonly Dictionary<string, List<TransactionInfo>> transactions = new();

    public List<TransactionInfo> GetTransactions(string accountId) {
        return transactions[accountId]
            .OrderBy((v) => v.DateStr)
            .ToList();
    }

    public TransactionInfo AddTransaction(string dateStr, string accountId, string action, decimal amount) {
        if (action == "W") {
            amount *= -1;
        }

        if (!balances.ContainsKey(accountId))
        {
            balances[accountId] = 0;
        }

        if (balances[accountId] + amount < 0) {
            throw new ArgumentException("Balance cannot be less than 0");
        }
        balances[accountId] += amount;

        string transactionId = CreateTransactionId(accountId, dateStr);
        TransactionInfo transaction = new(dateStr, transactionId, action, amount);

        if (!transactions.ContainsKey(accountId)) {
            transactions[accountId] = [];
        }
        transactions[accountId].Add(transaction);
        return transaction;
    }

    private string CreateTransactionId(string accountId, string dateStr) {
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
