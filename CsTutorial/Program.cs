using CsTutorial.Models;
using CsTutorial.Services;
using System.Globalization;
using System.Text.RegularExpressions;

TransactionService transactionService = new();
InterestService interestService = new();
bool shouldRun = true;

while (shouldRun) {
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

            DateTime date = DateTime.Now;
            if (!DateTime.TryParseExact(dateStr, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date)) {
                throw new ArgumentException("Wrong date format");
            }

            transactionService.AddTransaction(date, accountId, action, amount);
            IList<TransactionInfo> transactionInfos = transactionService.GetTransactions(accountId);
            Console.WriteLine($"Account: {accountId}\n{TransactionLogger.Log(transactionInfos)}");
            break;
        case "Q":
            shouldRun = false;
            Console.WriteLine("Thank you for banking with AwesomeGIC Bank.\r\nHave a nice day!");
            break;
        case "I":
            Console.WriteLine("Please enter interest rules details in <Date> <RuleId> <Rate in %> format \r\n(or enter blank to go back to main menu):");
            input = Console.ReadLine() ?? "";
            details = input.Split(" ");

            dateStr = details[0];
            string ruleId = details[1];
            string interestRateStr = details[2];
            decimal interestRate = decimal.Parse(interestRateStr);

            date = DateTime.Now;
            if (!DateTime.TryParseExact(dateStr, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date)) {
                throw new ArgumentException("Wrong date format");
            }

            interestService.AddInterest(date, ruleId, interestRate);
            IList<InterestInfo> interests = interestService.GetInterests();
            Console.WriteLine($"Interest rules: \n{InterestLogger.Log(interests)}");
            break;
        case "P":
            Console.WriteLine("Please enter account and month to generate the statement <Account> <Year><Month>\r\n(or enter blank to go back to main menu):");
            input = Console.ReadLine() ?? "";
            details = input.Split(" ");

            accountId = details[0];
            dateStr = details[1];
            string year = dateStr[..4];
            string month = dateStr.Substring(4, 2);

            transactionInfos = transactionService.GetTransactions(accountId)
                .Where((info) => info.Date.Year == int.Parse(year) && info.Date.Month == int.Parse(month))
                .ToList(); ;
            Console.WriteLine($"Account: {accountId}\n{TransactionLogger.Log(transactionInfos)}");
            break;
        default:
            continue;
    }


}
