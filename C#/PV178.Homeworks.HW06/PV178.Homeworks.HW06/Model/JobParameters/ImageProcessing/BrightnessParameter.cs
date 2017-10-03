using PV178.Homeworks.HW06.Content;

namespace PV178.Homeworks.HW06.Model.JobParameters.ImageProcessing
{
    /// <summary>
    /// Parameter for brightness adjustment job
    /// </summary>
    public class BrightnessParameter : JobParameter
    {
        /// <summary>
        /// Brightness change
        /// </summary>
        public int BrightnessChange { get; private set; }

        /// <summary>
        /// Image to apply brightness adjustment on
        /// </summary>
        public static ImageParameter Image { get; private set; }

        public BrightnessParameter()
        {
            if (Image == null)
            {
                Image = new ImageParameter(Paths.Image02);
            }
        }

        /// <summary>
        /// Initializes brightness by given value 
        /// (if it has not been initialized yet)
        /// </summary>
        /// <param name="brightnessChange">Brightness change</param>
        public void InitBrightness(int brightnessChange)
        {
            if (BrightnessChange == 0)
            {
                BrightnessChange = brightnessChange;
            }
        }
    }
}
