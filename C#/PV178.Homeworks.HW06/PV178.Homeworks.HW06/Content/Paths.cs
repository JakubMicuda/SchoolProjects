using System;
using System.IO;

namespace PV178.Homeworks.HW06.Content
{
    /// <summary>
    /// Contains paths to files within the Content folder
    /// </summary>
    public static class Paths
    {
        public static string ContentFolderPath => Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\PV178.Homeworks.HW06\Content\"));

        public static string OutputFolderPath => ContentFolderPath + @"Output\";

        public static string GetOutputImageFullName(long jobId, string imgName, string jobDescription) => OutputFolderPath + $"JobID_{jobId.ToString("D3")}_{jobDescription}_{imgName}.jpg";

        public static string LogFilePath => OutputFolderPath + $"log.txt";

        public static string ImagesFolderPath => ContentFolderPath + @"Images\";

        public static string Image01 => ImagesFolderPath + "img01_large.jpg";

        public static string Image02 => ImagesFolderPath + "img02_large.jpg";

        public static string Result01 => ContentFolderPath + @"Results\ETMB73.txt";

        public static string Result02 => ContentFolderPath + @"Results\ETMB732.txt";
    
        public static string BatchProcessFolderPath => ContentFolderPath + @"BatchProcess\";

        public static string BatchProcessJob(string fileName) => BatchProcessFolderPath + $"{fileName}.txt";

        public static string GetResultFullPath(string name) => ContentFolderPath + @"Results\" + name + ".txt";
    }

}
