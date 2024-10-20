using CsTutorial.Models;
using System;

namespace CsTutorial.Services;

public class InterestService {
    private record BalanceInfo(DateTime Start, DateTime End, decimal Balance);
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
        interests[start] = new InterestInfo(ruleId, interestRate, start);

        prevInfo = interests[start];
        return interests[start];
    }

    private static bool IsValidInterest(decimal interestRate) {
        return interestRate > 0 && interestRate < 100;
    }

    public decimal CalculateInterest(IList<TransactionInfo> transactions, DateTime start, DateTime end) {
        List<InterestInfo> interestInfos = interests
            .Values
            .Where((v) => v.End == null || v.End >= start)
            .OrderBy((v) => v.Start)
            .ToList();

        List<BalanceInfo> balances = [];
        for (int i = 0; i < transactions.Count; i++) {
            bool inRange = start <= transactions[i].Date && transactions[i].Date <= end;
            if (inRange) {
                DateTime _start = transactions[i].Date;
                DateTime _end = i + 1 < transactions.Count ? transactions[i + 1].Date : transactions[i].Date;
                balances.Add(new BalanceInfo(_start, _end, transactions[i].Amount));
            }
        }

        balances = balances
            .OrderBy((v) => v.Start)
            .ToList();

        decimal total = 0;
        // 2 pointers, same direction
        // everytime there is an overlap, we add to total amount
        int indexI = 0; // index for interest
        int indexB = 0; // index for balance

        while (indexI < interestInfos.Count && indexB < balances.Count) {
            InterestInfo interest = interestInfos[indexI];        
            BalanceInfo balance = balances[indexB];

            bool isOverlap = interest.Start <= balance.End && balance.Start <= interest.End;
            if (isOverlap) {
                DateTime _start = new List<DateTime> { interest.Start , balance.Start }.Max();
                DateTime _end = new List<DateTime> { interest.End ?? new DateTime(), balance.End }.Min();
                int days = (_end - _start).Days;

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
