using System;
using System.Drawing;
using System.IO;
using PV178.Homeworks.HW05.Utils;

namespace PV178.Homeworks.HW05
{
    class Program
    {
        static void Main(string[] args)
        {
            if(TestBitMapSaving())
                Console.WriteLine("Bitmap saving is corrent");
            else
                Console.WriteLine("Bitmap saving is incorrent");
        }

        private static bool TestBitMapSaving()
        {
            string jpgPath =
                Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    @"..\..\..\PV178.Homeworks.HW05\Content\Map\map.jpg"));

            using (var bitmapStream = MapImageIO.LoadImgToStream(jpgPath))
            {
                Bitmap bmp = new Bitmap(bitmapStream);
                BitmapDrawer bmpDrawer = new BitmapDrawer(bitmapStream);

                using (var resultStream = bmpDrawer.SaveBitmapToStream())
                using (var expectedStream = new MemoryStream())
                {
                    bmp.Save(expectedStream,bmp.RawFormat);

                    if (resultStream.Length != expectedStream.Length)
                        return false;

                    int resultByte, expectedByte;

                    do
                    {
                        resultByte = resultStream.ReadByte();
                        expectedByte = expectedStream.ReadByte();

                        if (resultByte != expectedByte)
                            return false;
                    } while (resultByte != -1 && expectedByte != -1);

                    return true;
                }
            }
        }
    }
}
