using CommandLineTutorial.Handlers;
using CommandLineTutorial.Services;

TransactionService transactionService = new();
InterestService interestService = new();

MainHandler mainHandler = new(interestService, transactionService);
mainHandler.Run();