using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using PV178.Homeworks.HW05.Model;
using PV178.Homeworks.HW05.Utils;

namespace PV178.Homeworks.HW05.Parsers.GpxParsers
{
    public class GpxJsonParser : BaseParser
    {
        protected override IList<GpsCoordinates> ParseData(string data)
        {
            return JsonConvert.DeserializeObject<List<GpsCoordinates>>(data);
        }
    }
}
