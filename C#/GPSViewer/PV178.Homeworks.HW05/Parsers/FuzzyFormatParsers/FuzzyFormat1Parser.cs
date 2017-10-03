using PV178.Homeworks.HW05.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using PV178.Homeworks.HW05.Utils;

namespace PV178.Homeworks.HW05.Parsers.FuzzyFormatParsers
{
    public class FuzzyFormat1Parser : BaseFuzzyParser
    {
        public FuzzyFormat1Parser()
        {
            Regexes = new List<Regex>();
            Regexes.Add(new Regex(@"^(?<latitude>\d+\.\d+) \w+ \w+; (?<longitude>\d+\.\d+) \w+ \w+$"));
            Regexes.Add(new Regex(@"^\w+ \w+ \w+ \w+: \d+\.[0]+:(?<latitude>\d+\.\d+),(?<longitude>\d+\.\d+)$"));
            Regexes.Add(new Regex(@"^(?<latitude>\d+\.\d+)E:(?<longitude>\d+\.\d+)N$"));
            Regexes.Add(new Regex(@"^(?<latitude>\d+\.\d+) \w+, (?<longitude>\d+\.\d+) \w+,$"));
            Regexes.Add(new Regex(@"^(?<latitude>\d+\.\d+);(?<longitude>\d+\.\d+)$"));
        }
    }
}
