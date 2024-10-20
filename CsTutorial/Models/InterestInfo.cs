using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsTutorial.Models;
public record InterestInfo(string DateStr, string Id, decimal InterestRate) {
    public string DateStringFormat { get; } = "yyyyMMdd";
}
