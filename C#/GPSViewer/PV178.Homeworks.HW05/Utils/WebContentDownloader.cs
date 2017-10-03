using System.IO;
using System.Net;

namespace PV178.Homeworks.HW05.Utils
{
    /// <summary>
    /// Utility class for downloading content from web
    /// </summary>
    public static class WebContentDownloader
    {
        private const string Route05Url = "http://is.muni.cz/www/409727/64666522/64666524/route05.gpx";

        /// <summary>
        /// Downloads file: route05.gpx from <see cref="Route05Url"/>
        /// </summary>
        /// <param name="filePath">Determines where downloaded file will be saved.</param>
        public static void DownloadContent(string filePath)
        {
            var req = (HttpWebRequest)WebRequest.Create(Route05Url);
            using (var resp = (HttpWebResponse) req.GetResponse())
            {
                var dataStream = resp.GetResponseStream();
                using (var fileStream = File.Create(filePath))
                {
                    SaveResponse(fileStream,dataStream);
                }

            }           
        }

        /// <summary>
        /// Saves dataStream to fileStream
        /// </summary>
        /// <param name="fileStream">File stream to save data to</param>
        /// <param name="dataStream">Stream with GPX data</param>
        private static void SaveResponse(FileStream fileStream, Stream dataStream)
        {
            dataStream.CopyTo(fileStream);
        }
    }
}
