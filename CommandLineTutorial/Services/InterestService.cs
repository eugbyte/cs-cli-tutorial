using CommandLineTutorial.Models;

namespace CommandLineTutorial.Services;

public class InterestService {
	public record BalanceInfo(DateTime Start, DateTime End, decimal Balance);
	private InterestInfo? prevInfo = null;
	// Date against InterestInfo
	private readonly Dictionary<DateTime, InterestInfo> interests = new();

	public List<InterestInfo> GetInterests() {
		return interests
			.Values
			.OrderBy((v) => v.Start)
			.ToList();
	}

	public InterestInfo AddInterest(DateTime start, string ruleId, decimal interestRate) {
		if (!IsValidInterest(interestRate)) {
			throw new ArgumentException("Invalid interest rate");
		}
		if (prevInfo != null) {
			prevInfo = prevInfo with {
				End = start
			};
			interests[prevInfo.Start] = prevInfo;
		}
		interests[start] = new InterestInfo(ruleId, interestRate, start, DateTime.Now);

		prevInfo = interests[start];
		return interests[start];
	}

	private static bool IsValidInterest(decimal interestRate) {
		return interestRate > 0 && interestRate < 100;
	}

	public decimal CalculateInterest(IList<TransactionInfo> transactions, DateTime start, DateTime end) {
		List<BalanceInfo> balances = [];
		for (int i = 0; i < transactions.Count; i++) {
			TransactionInfo transactionInfo = transactions[i];
			if (start <= transactions[i].Date && transactions[i].Date <= end) {
				DateTime _start = transactions[i].Date;
				DateTime _end = i + 1 < transactions.Count ? transactions[i + 1].Date : end;
				balances.Add(new BalanceInfo(_start, _end, transactions[i].Amount));
			}
		}

		List<InterestInfo> interestInfos = interests
			.Values
			.Where((v) => v.Start <= end && start <= v.End)
			.OrderBy((v) => v.Start)
			.ToList();

		if (interestInfos.Count > 0 && balances.Count > 0) {
			DateTime minInterestStart = new List<DateTime> { interestInfos[0].Start, balances[0].Start }.Min();
			interestInfos[0] = interestInfos[0] with { Start = minInterestStart };
		}

		decimal total = 0;
		// 1. Two pointers, same direction
		// 2. Find overlaps between the two list of intervals
		// 3. Everytime there is an overlap, we calculate interest and add to total amount
		int indexI = 0; // index for interest

		int indexB = 0; // index for balance

		while (indexI < interestInfos.Count && indexB < balances.Count) {
			InterestInfo interest = interestInfos[indexI];
			BalanceInfo balance = balances[indexB];

			bool isOverlap = interest.Start <= balance.End && balance.Start <= interest.End;
			if (isOverlap) {
				DateTime overlapStart = new List<DateTime> { interest.Start, balance.Start }.Max();
				DateTime overlapEnd = new List<DateTime> { interest.End, balance.End }.Min();
				int days = (overlapEnd - overlapStart).Days;

				decimal amount = balance.Balance * interest.InterestRate * days;
				amount /= 365;
				total += amount;
			}

			// increment the interval which ends earlier as there is a chance for more overlaps
			if (interest.End < balance.End) {
				indexI += 1;
			} else {
				indexB += 1;
			}
		}
		return total;
	}
}
