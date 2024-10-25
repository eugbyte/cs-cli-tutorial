using CommandLineTutorial.Domains.Models;

namespace CommandLineTutorial.Domains.Interfaces;
public interface ITransactionService {
	List<TransactionInfo> GetTransactions(string accountId);
	TransactionInfo AddTransaction(DateTime date, string accountId, ACTION action, decimal amount);

}
