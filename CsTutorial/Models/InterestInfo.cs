using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsTutorial.Models;
public record InterestInfo(DateTime Date, string Id, decimal InterestRate) {
    public static string DateStringFormat { get; } = "yyyyMMdd";
}
