using System.Globalization;

namespace CsTutorial.Models;

public record class TransactionInfo(string Date, string Id, string Type, decimal Amount) {
    public string DateStringFormat { get; } = "yyyyMMdd";
};