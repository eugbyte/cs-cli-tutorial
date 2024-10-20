using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsTutorial.Models;
public record InterestInfo(string Id, decimal InterestRate, DateTime Start, DateTime? End = null) {
    public static string DateStringFormat { get; } = "yyyyMMdd";
}
