using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PV178.Homeworks.HW05.Model;
using PV178.Homeworks.HW05.Utils;

namespace PV178.Homeworks.HW05.Parsers.FuzzyFormatParsers
{
    public abstract class BaseFuzzyParser : BaseParser
    {
        protected List<Regex> Regexes { get; set; }

        protected override IList<GpsCoordinates> ParseData(string data)
        {
            List<GpsCoordinates> gpsPoints = new List<GpsCoordinates>();

            using (StringReader sr = new StringReader(data))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    try
                    {
                        Match match = Regexes.First(reg => reg.IsMatch(line)).Match(line);

                        double latitude = Helpers.ParseDouble(match.Groups["latitude"].Value);
                        double longitude = Helpers.ParseDouble(match.Groups["longitude"].Value);

                        gpsPoints.Add(new GpsCoordinates(latitude, longitude));
                    }
                    catch (InvalidOperationException ex)
                    {
                        throw new InvalidOperationException("[FailedLine]:" + line);
                    }

    
                }

                return gpsPoints;
            }
        }
    }
}
