using System.Globalization;

namespace CsTutorial.Models;

public record class TransactionInfo(DateTime Date, string Id, string Action, decimal Amount) {
    public static string DateStringFormat { get; } = "yyyyMMdd";
};