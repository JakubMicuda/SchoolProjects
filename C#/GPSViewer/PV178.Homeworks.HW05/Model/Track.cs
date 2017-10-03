using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PV178.Homeworks.HW05.Utils;

namespace PV178.Homeworks.HW05.Model
{
    /// <summary>
    /// Represents Route, consisting of many GPS coordinates
    /// </summary>
    public class Track
    {
        /// <summary>
        /// Recorded GPS points
        /// </summary>
        public IList<GpsCoordinates> GPSPoints { get; }

        /// <summary>
        /// Pixel coordinates (within map image) of the respective GPS points
        /// </summary>
        public IList<Point> ImagePoints { get; } = new List<Point>();

        public Track(IList<GpsCoordinates> gpsPoints)
        {
            GPSPoints = gpsPoints;

            var converter = new GpsToImageOffsetConverter();
            foreach (var point in gpsPoints
                .Select(gpsPoint => converter.ConvertGpsToImageOffset(gpsPoint)))
            {
                ImagePoints.Add(point);
            }
        }
    }
}
