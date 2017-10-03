using PV178.Homeworks.HW06.Jobs;
using PV178.Homeworks.HW06.Jobs.ImageProcessing;
using PV178.Homeworks.HW06.Model.JobParameters;
using PV178.Homeworks.HW06.Model.JobParameters.ImageProcessing;
using PV178.Homeworks.HW06.Model.JobResults.ImageProcessing;
using System;

namespace PV178.Homeworks.HW06.Utils.JobBuilders.ImageProcessing
{
    /// <summary>
    /// Brightness job builder
    /// </summary>
    public class BrightnessJobBuilder : JobBuilder<BrightnessParameter, BrightnessResult>
    {
        protected override void SetArgumentCore(string customParameter)
        {
            BrightnessParameter arg = (BrightnessParameter)Job.Argument;

            int brightnessChange;

            try
            {
                brightnessChange = int.Parse(customParameter);
            }
            catch (Exception)
            {
                brightnessChange = JobSettings.DefaultBrightnessAdjustment;
            }

            if (brightnessChange > JobSettings.MaxBrightness)
                brightnessChange = JobSettings.MaxBrightness;
            else if (brightnessChange < JobSettings.MinBrightness)
                brightnessChange = JobSettings.MinBrightness;

            arg.InitBrightness(brightnessChange);
        }

        protected override void SetWork()
        {
            Job.InitWork(ImageProcess.EditBrightness);
        }
    }
}
