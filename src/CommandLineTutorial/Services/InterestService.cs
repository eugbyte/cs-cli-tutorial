using CommandLineTutorial.Domains.Interfaces;
using CommandLineTutorial.Domains.Models;

namespace CommandLineTutorial.Services;

public class InterestService : IInterestService {
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
		start = new DateTime(year: start.Year, month: start.Month, day: start.Day);

		if (!IsValidInterest(interestRate)) {
			throw new ArgumentException("Invalid interest rate");
		}
		if (prevInfo != null) {
			prevInfo = prevInfo with {
				End = start.AddDays(-1)
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
				DateTime _end = end;
				if (i + 1 < transactions.Count) {
					_end = transactions[i + 1].Date.AddDays(-1);
				}
				balances.Add(new BalanceInfo(_start, _end, transactions[i].LatestBalance));
			}
		}

		List<InterestInfo> interestInfos = interests
			.Values
			.Where((v) => v.Start <= end && start <= v.End)
			.Select((v) => v with { End = new List<DateTime> { v.End, end }.Min() })
			.OrderBy((v) => v.Start)
			.ToList();

		return SumPeriodicInterests(balances, interestInfos);
	}

	// 1. Find overlaps between the two list of intervals
	// 2. Two pointers, same direction
	// 3. Everytime there is an overlap, we calculate interest and add to total amount
	private static decimal SumPeriodicInterests(List<BalanceInfo> balances, List<InterestInfo> interestInfos) {
		decimal total = 0;
		int indexI = 0; // index for interest
		int indexB = 0; // index for balance

		while (indexI < interestInfos.Count && indexB < balances.Count) {
			InterestInfo interest = interestInfos[indexI];
			BalanceInfo balance = balances[indexB];

			bool isOverlap = interest.Start <= balance.End && balance.Start <= interest.End;
			if (isOverlap) {
				DateTime overlapStart = new List<DateTime> { interest.Start, balance.Start }.Max();
				DateTime overlapEnd = new List<DateTime> { interest.End, balance.End }.Min();
				int days = (overlapEnd - overlapStart).Days + 1;

				decimal amount = balance.Balance * (interest.InterestRate / 100) * days;
				total += amount;
			}

			// increment the interval which ends earlier as there is a chance for more overlaps
			if (interest.End < balance.End) {
				indexI += 1;
			} else {
				indexB += 1;
			}
		}

		total /= 365;
		return total;
	}
}
