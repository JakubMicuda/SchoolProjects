using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PV178.Homeworks.HW05.Parsers.FuzzyFormatParsers
{
    public class FuzzyFormat2Parser : BaseFuzzyParser
    {
        public FuzzyFormat2Parser()
        {
            Regexes = new List<Regex>();
            Regexes.Add(new Regex(@"^(?<latitude>\d+\.\d+) lat\w*, (?<longitude>\d+\.\d+) lon\w*$"));
            Regexes.Add(
                new Regex(
                    @"^\w+ \w+: \d+\.\d+ \w+  \d+\.\d+ \w+ \w+ (?<latitude>\d+\.\d+) \w+ \w+: \d+\.\d+ \w+  \d+\.\d+ \w+ \w+ (?<longitude>\d+\.\d+) $"));
            Regexes.Add(new Regex(@"^(?<longitude>\d+\.\d+) lon\w*, (?<latitude>\d+\.\d+) lat\w*$"));
            Regexes.Add(new Regex(@"^(?<latitude>\d+\.\d+)E0\.00000\.0(?<longitude>\d+\.\d+)N$"));
            Regexes.Add(new Regex(@"^(?<latitude>\d+\.\d+)\.(?<longitude>\d+\.\d+)$"));
        }
    }
}
