﻿using CsTutorial.Models;
using CsTutorial.Services;
using System.Globalization;

TransactionService transactionService = new();

while (true) {
    Console.WriteLine(@"
Welcome to Awesome Bank! What would you like to do?
[T] Input transactions 
[I] Define interest rules
[P] Print statement
[Q] Quit");

    string input = (Console.ReadLine() ?? "").ToUpper();
    switch(input) {
        case "T":
            Console.WriteLine("Please enter transaction details in <Date> <Account> <Type> <Amount> format");
            input = Console.ReadLine() ?? "";
            string[] details = input.Split(" ");

            if (details.Length != 4) {
                throw new ArgumentException("Wrong input format");
            }

            string dateStr = details[0];
            string accountId = details[1];
            string action = details[2];
            string amountStr = details[3];
            decimal amount = decimal.Parse(amountStr);

            transactionService.AddTransaction(dateStr, accountId, action, amount);
            IList<TransactionInfo> transactionInfos = transactionService.GetTransactions(accountId);
            Console.WriteLine($"Account: {accountId}\n{TransactionLogger.Log(transactionInfos)}");
            break;
        case "Q":
            break;
        case "I":
            Console.WriteLine("Please enter interest rules details in <Date> <RuleId> <Rate in %> format \r\n(or enter blank to go back to main menu):");
            input = Console.ReadLine() ?? "";
            details = input.Split(" ");

            break;
        default:
            continue;
    }


}
