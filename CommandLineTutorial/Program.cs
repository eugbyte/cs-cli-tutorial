using CommandLineTutorial.Handlers;
using CommandLineTutorial.Models;
using CommandLineTutorial.Services;
using System;
using CommandLineTutorial.Services.Loggers;

TransactionService transactionService = new();
InterestService interestService = new();

string accountId = "AC001";

transactionService.AddTransaction(new DateTime(year: 2024, month: 6, day: 26), accountId, action: "D", 200);
List<TransactionInfo> transactions = transactionService.GetTransactions(accountId);
TransactionLogger.Log(accountId, transactions);

interestService.AddInterest(new DateTime(year: 2024, month: 6, day: 26), "RULE03", (decimal)(2.20));
List<InterestInfo> interests = interestService.GetInterests();
InterestLogger.Log(interests);

List<TransactionInfo> transactionInfos = transactionService.GetTransactions(accountId);

DateTime start = new(year: 2024, month: 6, day: 1);
DateTime end = new(year: 2024, month: 6, day: 30);

decimal interest = interestService.CalculateInterest(transactionInfos, start, end);
Console.WriteLine($"interest: {interest:0.00}");

BalanceLoger.Log(transactionInfos, end, interest);

//MainHandler mainHandler = new(interestService, transactionService);
//mainHandler.Run();