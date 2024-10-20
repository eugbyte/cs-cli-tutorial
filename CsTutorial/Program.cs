using CsTutorial.Models;
using CsTutorial.Services;
using System.Globalization;

while (true) {
    const string optionsText = @"
        Welcome to AwesomeGIC Bank! What would you like to do?
        [T] Input transactions 
        [I] Define interest rules
        [P] Print statement
        [Q] Quit
    ";

    Console.WriteLine(optionsText);

    string input = (Console.ReadLine() ?? "").ToUpper();
    switch(input) {
        case "T":
            Console.WriteLine("Please enter transaction details in <Date> <Account> <Type> <Amount> format");
            input = (Console.ReadLine() ?? "");
            string[] details = input.Split(" ");

            if (details.Length != 4) {
                Console.WriteLine($"Wrong input format");
                continue;
            }

            string dateStr = details[0];
            string accountId = details[1];
            string action = details[2];
            string amountStr = details[3];
            decimal amount = decimal.Parse(amountStr);

            break;
        default:
            continue;
    }


}
