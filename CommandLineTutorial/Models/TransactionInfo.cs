﻿namespace CommandLineTutorial.Models;

public record TransactionInfo(DateTime Date, string Id, string Action, decimal Amount, decimal LatestBalance) {
    public static string DateStringFormat { get; } = "yyyyMMdd";
};