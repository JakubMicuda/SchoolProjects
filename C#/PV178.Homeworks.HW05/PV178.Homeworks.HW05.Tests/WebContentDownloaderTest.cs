using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PV178.Homeworks.HW05.Utils;

namespace PV178.Homeworks.HW05.Tests
{
    [TestClass]
    public class WebContentDownloaderTest
    {
        [TestMethod]
        public void WebContentDownloader_DownloadGpxFile_FileExists()
        {
            File.Delete(TestPaths.Route05);
            WebContentDownloader.DownloadContent(TestPaths.Route05);
            Assert.AreEqual(true, File.Exists(TestPaths.Route05), "File: route05.gpx was not downloaded");
        }
    }
}
