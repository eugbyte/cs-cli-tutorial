using CommandLineTutorial.Domains.Models;

namespace CommandLineTutorial.Domains.Interfaces;
public interface IInterestService {
	List<InterestInfo> GetInterests();
	InterestInfo AddInterest(DateTime start, string ruleId, decimal interestRate);
	decimal CalculateInterest(IList<TransactionInfo> transactions, DateTime start, DateTime end);
}
