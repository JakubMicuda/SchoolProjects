using System.Globalization;

namespace PV178.Homeworks.HW05.Model
{
    /// <summary>
    /// Represent GPS coordinates, http://www.gisdoctor.com/site/wp-content/uploads/2015/08/latlong_final.jpg
    /// </summary>
    public struct GpsCoordinates
    {
        public double Latitude { get; }
        public double Longitude { get; }

        public GpsCoordinates(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public override string ToString()
        {
            return $"[{Latitude.ToString(CultureInfo.InvariantCulture)};{Longitude.ToString(CultureInfo.InvariantCulture)}]";
        }

        public bool Equals(GpsCoordinates other)
        {
            return Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is GpsCoordinates && Equals((GpsCoordinates) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Latitude.GetHashCode()*397) ^ Longitude.GetHashCode();
            }
        }
    }
}
