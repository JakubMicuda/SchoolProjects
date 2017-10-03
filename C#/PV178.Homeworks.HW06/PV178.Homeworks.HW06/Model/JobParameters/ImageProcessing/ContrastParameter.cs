using PV178.Homeworks.HW06.Content;

namespace PV178.Homeworks.HW06.Model.JobParameters.ImageProcessing
{
    /// <summary>
    /// Parameter for contrast adjustment job
    /// </summary>
    public class ContrastParameter : JobParameter
    {
        /// <summary>
        /// Contrast change
        /// </summary>
        public int ContrastChange { get; private set; }

        /// <summary>
        /// Image to apply contrast adjustment on
        /// </summary>
        public static ImageParameter Image { get; private set; }

        public ContrastParameter()
        {
            if (Image == null)
            {
                Image = new ImageParameter(Paths.Image01);
            }
        }

        /// <summary>
        /// Initializes contrast by given value 
        /// (if it has not been initialized yet)
        /// </summary>
        /// <param name="contrastChange">Contrast change</param>
        public void InitContrast(int contrastChange)
        {
            if (ContrastChange == 0)
            {
                ContrastChange = contrastChange;
            }
        }
    }
}
