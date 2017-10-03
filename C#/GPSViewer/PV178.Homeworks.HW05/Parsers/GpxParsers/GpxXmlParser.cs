using System;
using System.IO;
using System.Xml.Linq;
using PV178.Homeworks.HW05.Model;
using System.Collections.Generic;
using System.Linq;
using PV178.Homeworks.HW05.Utils;

namespace PV178.Homeworks.HW05.Parsers.GpxParsers
{
    public class GpxXmlParser : BaseParser
    {
        #region XmlElementNames

        private const string TrackElement = "{http://www.topografix.com/GPX/1/1}trk";

        private const string TrackSequenceElement = "{http://www.topografix.com/GPX/1/1}trkseg";

        private const string TrackpointElement = "{http://www.topografix.com/GPX/1/1}trkpt";

        #endregion

        protected override IList<GpsCoordinates> ParseData(string data)
        {
            XDocument doc = XDocument.Parse(data);

            return doc.Root.Element(TrackElement)
                    .Elements(TrackSequenceElement)
                    .Elements(TrackpointElement)
                    .Select(
                        el =>
                            new GpsCoordinates(Helpers.ParseDouble((string)el.Attribute("lat")),
                                Helpers.ParseDouble((string)el.Attribute("lon"))))
                    .ToList();
        }
    }
}
