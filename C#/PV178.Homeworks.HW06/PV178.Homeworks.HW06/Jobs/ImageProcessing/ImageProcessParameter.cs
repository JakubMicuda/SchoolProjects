using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PV178.Homeworks.HW06.Model.Enums;
using PV178.Homeworks.HW06.Model.JobParameters;
using PV178.Homeworks.HW06.Model.JobParameters.ImageProcessing;
using PV178.Homeworks.HW06.Model.JobResults;
using PV178.Homeworks.HW06.Model.JobResults.ImageProcessing;

namespace PV178.Homeworks.HW06.Jobs.ImageProcessing
{
    public delegate byte ByteChange(byte data);

    public class ImageProcessParameter
    {
        public JobResult Result { get; private set; }
        public ImageParameter Image { get; private set; }
        public ByteChange PixelAction { get; private set; }
        public int Value { get; private set; }
        public string Description { get; private set; }

        public ImageProcessParameter(JobParameter param, JobType type)
        {
            switch (type)
            {
                case JobType.ImageProcessingBrightness:
                    Image = BrightnessParameter.Image;
                    PixelAction = AdjustBrightness;
                    Value = ((BrightnessParameter) param).BrightnessChange;
                    Result = new BrightnessResult(Value);
                    Description = "Brightness changed";
                break;

                case JobType.ImageProcessingContrast:
                    Image = ContrastParameter.Image;
                    PixelAction = AdjustContrast;
                    Value = ((ContrastParameter)param).ContrastChange;
                    Result = new ContrastResult(Value);
                    Description = "Contrast changed";
                break;

                default:
                    throw new ArgumentException("invalid image process type");
            }

        }

        private byte AdjustContrast(byte subpixel)
        {
            double change = Math.Pow((100 + Value) / 100.0, 2);
            double doubleResult = ((subpixel / 255.0 - 0.5) * change + 0.5) * 255;

            if (doubleResult > byte.MaxValue)
                return byte.MaxValue;
            if (doubleResult < byte.MinValue)
                return byte.MinValue;

            return (byte)doubleResult;
        }

        private byte AdjustBrightness(byte subpixel)
        {
            int intResult = subpixel + Value;

            if (intResult > byte.MaxValue)
                return byte.MaxValue;
            if (intResult < byte.MinValue)
                return byte.MinValue;

            return (byte)intResult;
        }

    }
}
