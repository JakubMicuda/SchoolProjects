using System.Drawing;
using System.Drawing.Imaging;

namespace PV178.Homeworks.HW06.Model.JobParameters.ImageProcessing
{
    /// <summary>
    /// Wrapper class for loaded image
    /// </summary>
    public class ImageParameter
    {
        /// <summary>
        /// Image width in pixels
        /// </summary>
        public int ImgWidth { get; }

        /// <summary>
        /// Image height in pixels
        /// </summary>
        public int ImgHeight { get; private set; }

        /// <summary>
        /// Image pixel format
        /// </summary>
        public PixelFormat ImgPixelFormat { get; }

        /// <summary>
        /// Number of bytes per pixel
        /// </summary>
        public int BytesPerPixel { get; }

        /// <summary>
        /// Number of bytes per pixel row
        /// </summary>
        public int Stride { get; private set; }


        private readonly Bitmap bitmap;
        /// <summary>
        /// Bitmap storing the image
        /// </summary>
        public Bitmap CreateBitmapDeepCopy() => new Bitmap(bitmap);

        public ImageParameter(string imgPath)
        {
            bitmap = new Bitmap(Image.FromFile(imgPath));
            ImgWidth = bitmap.Width;
            ImgHeight = bitmap.Height;
            ImgPixelFormat = bitmap.PixelFormat;
            BytesPerPixel = Image.GetPixelFormatSize(ImgPixelFormat) / 8;
            Stride = BytesPerPixel * ImgWidth;
        }

        ~ImageParameter()
        {
            bitmap?.Dispose();
        }
    }
}
