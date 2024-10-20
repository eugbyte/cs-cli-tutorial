using System.Globalization;

namespace CsTutorial.Models;

public record class TransactionInfo(string DateStr, string Id, string Action, decimal Amount) {
    public string DateStringFormat { get; } = "yyyyMMdd";
};