using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PV178.Homeworks.HW05.Model;
using PV178.Homeworks.HW05.Utils;

namespace PV178.Homeworks.HW05.Parsers
{
    public abstract class BaseParser : IGpsParser
    {
        public Track ParseTrack(byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                return ParseTrack(stream);
            }
        }

        public Track ParseTrack(Stream stream)
        {
            string data = string.Empty;

            using (StreamReader reader = new StreamReader(stream))
            {
                data = reader.ReadToEnd();
            }

            IList<GpsCoordinates> gpsPoints = ParseData(data);

            return new Track(gpsPoints);
        }

        public Track ParseTrack(string filePath)
        {
            using (Stream stream = MapImageIO.LoadImgToStream(filePath))
            {
                return ParseTrack(stream);
            }
        }

        protected virtual IList<GpsCoordinates> ParseData(string data)
        {
            return new List<GpsCoordinates>();
        }
    }
}
