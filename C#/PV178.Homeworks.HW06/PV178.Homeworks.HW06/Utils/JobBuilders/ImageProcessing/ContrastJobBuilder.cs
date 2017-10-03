using System;
using PV178.Homeworks.HW06.Jobs;
using PV178.Homeworks.HW06.Jobs.ImageProcessing;
using PV178.Homeworks.HW06.Model.JobParameters.ImageProcessing;
using PV178.Homeworks.HW06.Model.JobResults.ImageProcessing;
using PV178.Homeworks.HW06.Model.JobParameters;

namespace PV178.Homeworks.HW06.Utils.JobBuilders.ImageProcessing
{
    /// <summary>
    /// Contrast job builder
    /// </summary>
    public class ContrastJobBuilder : JobBuilder<ContrastParameter, ContrastResult>
    {
        // TODO
        protected override void SetArgumentCore(string customParameter)
        {
            ContrastParameter arg = (ContrastParameter)Job.Argument;

            int contrastChange;

            try
            {
                contrastChange = int.Parse(customParameter);
            }
            catch (Exception)
            {
                contrastChange = JobSettings.DefaultContrastAdjustment;
            }

            if (contrastChange > JobSettings.MaxContrast)
                contrastChange = JobSettings.MaxContrast;
            else if (contrastChange < JobSettings.MinContrast)
                contrastChange = JobSettings.MinContrast;

            arg.InitContrast(contrastChange);
        }

        protected override void SetWork()
        {
            Job.InitWork(ImageProcess.EditContrast);
        }
    }
}
