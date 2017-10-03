using System.IO;
using PV178.Homeworks.HW05.Model;

namespace PV178.Homeworks.HW05.Parsers
{
    /// <summary>
    /// Common interface for all GPS coordinate parsers
    /// </summary>
    public interface IGpsParser
    {
        /// <summary>
        /// Parses track from GPS data file with given path
        /// </summary>
        /// <param name="filePath">Full GPS data file path</param>
        /// <returns>Extracted Track</returns>
        Track ParseTrack(string filePath);

        /// <summary>
        /// Parses track from given stream containing GPS data
        /// </summary>
        /// <param name="stream">GPS data stream</param>
        /// <returns>Extracted Track</returns>
        Track ParseTrack(Stream stream);

        /// <summary>
        /// Parses track from byte array stream containing GPS data
        /// </summary>
        /// <param name="bytes">GPS data byte array</param>
        /// <returns>Extracted Track</returns>
        Track ParseTrack(byte[] bytes);
    }
}
