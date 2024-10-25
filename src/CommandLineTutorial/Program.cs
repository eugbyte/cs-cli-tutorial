using CommandLineTutorial.Handlers;
using CommandLineTutorial.Models;
using CommandLineTutorial.Services;
using System;
using CommandLineTutorial.Services.Loggers;

TransactionService transactionService = new();
InterestService interestService = new();

string accountId = "AC001";
string actionStr = "D";
_ = Enum.TryParse(actionStr, out ACTION action);

transactionService.AddTransaction(new DateTime(year: 2024, month: 6, day: 1), accountId, action, amount: 200);
List<TransactionInfo> transactions = transactionService.GetTransactions(accountId);
TransactionLogger.Log(accountId, transactions);
Console.WriteLine();

interestService.AddInterest(new DateTime(year: 2024, month: 6, day: 1), "RULE03", (decimal)(2.20));
List<InterestInfo> interests = interestService.GetInterests();
InterestLogger.Log(interests);
Console.WriteLine();

List<TransactionInfo> transactionInfos = transactionService.GetTransactions(accountId);

DateTime start = new(year: 2024, month: 6, day: 1);
// To Do - handle February and leap year
DateTime end = new(year: 2024, month: 6, day: 30);

decimal interest = interestService.CalculateInterest(transactionInfos, start, end);
Console.WriteLine($"interest: {interest:0.00}");
Console.WriteLine();

BalanceLoger.Log(transactionInfos, end, interest);

//MainHandler mainHandler = new(interestService, transactionService);
//mainHandler.Run();