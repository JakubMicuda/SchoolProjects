using System;
using System.IO;

namespace PV178.Homeworks.HW05.Content
{
    /// <summary>
    /// Contains paths to files within the Content folder
    /// </summary>
    public static class Paths
    {
        public static string ContentFolderPath => Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Content\"));

        public static string OutputFolderPath => ContentFolderPath + @"Output\";

        public static string OutputMap01 => OutputFolderPath + "map01";

        public static string OutputMap02 => OutputFolderPath + "map02";

        public static string OutputMap03 => OutputFolderPath + "map03";

        public static string OutputMap04 => OutputFolderPath + "map04";

        public static string OutputMap05 => OutputFolderPath + "map05";

        public static string RoutesFolderPath => ContentFolderPath + @"Routes\";

        public static string Route01 => RoutesFolderPath + "Route01.json";

        public static string Route02 => RoutesFolderPath + "Route02.gpx";

        public static string Route03 => RoutesFolderPath + "Route03.ff1";

        public static string Route04 => RoutesFolderPath + "Route04.ff2";

        public static string Route05 => RoutesFolderPath + "Route05.gpx";

        public static string MapImgPath => ContentFolderPath + @"Map\map.jpg";
    }
}
