using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace PV178.Homeworks.HW05.Utils
{
    /// <summary>
    /// Utility class for image I/O operations
    /// </summary>
    public class MapImageIO
    {
        public static ImageFormat MapImgFormat => ImageFormat.Jpeg;

        /// <summary>
        /// Loads image to stream
        /// </summary>
        /// <param name="filePath">The full path of the image</param>
        /// <returns>Stream with image data</returns>
        public static Stream LoadImgToStream(string filePath)
        {
            return File.Open(filePath, FileMode.Open);
        }

        /// <summary>
        /// Saves image to file, to set desired quality, see this link:
        /// https://msdn.microsoft.com/en-us/library/bb882583(v=vs.110).aspx
        /// </summary>
        /// <param name="stream">Stream with image data</param>
        /// <param name="outputPath">Output image path (without file name)</param>
        /// <param name="fileName">Image file name</param>
        public static void SaveImgToFile(Stream stream, string outputPath, string fileName)
        {
            if (stream == null)
                return;

            using (Bitmap bmp = new Bitmap(stream))
            {
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                Encoder myEncoder = Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);

                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 100L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                bmp.Save(outputPath + fileName, jpgEncoder, myEncoderParameters);
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}
