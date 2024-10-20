using CsTutorial.Models;

namespace CsTutorial.Services;

public  class TransactionLogger {
    public static string Log(IList<TransactionInfo> transactionInfos) {
        const string fmt = "|{0, -10} | {1, -15} | {2, -10} | {3, -10}|";
        string header = string.Format(fmt, "Date", "Txn Id", "Type", "Amount");
        List<string> rows = transactionInfos
            .Select(info => string.Format(fmt, info.DateStr, info.Id, info.Action, info.Amount))
            .ToList();
        return string.Join("\n", [header, .. rows]);
    }
}
