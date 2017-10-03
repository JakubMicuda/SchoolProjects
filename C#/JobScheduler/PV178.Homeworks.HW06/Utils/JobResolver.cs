using System;
using PV178.Homeworks.HW06.Model;
using PV178.Homeworks.HW06.Model.Enums;
using PV178.Homeworks.HW06.Model.JobParameters;
using PV178.Homeworks.HW06.Model.JobResults;
using PV178.Homeworks.HW06.Utils.JobBuilders;
using PV178.Homeworks.HW06.Utils.JobBuilders.DataSorting;
using PV178.Homeworks.HW06.Utils.JobBuilders.ImageProcessing;

namespace PV178.Homeworks.HW06.Utils
{
    /// <summary>
    /// Simple class which handles job resolving
    /// </summary>
    public class JobResolver
    {
        /// <summary>
        /// Resolves job according to its type
        /// </summary>
        /// <param name="scheduleParams">Job initialization params</param>
        /// <param name="jobType">Type of job to build</param>
        /// <returns></returns>
        public static Job Resolve(ScheduleParams scheduleParams, JobType jobType)
        {
            if (scheduleParams == null)
            {
                throw new ArgumentNullException(nameof(scheduleParams));
            }

            // TODO: use appropriate job builder according to jobType here

            Job newJob = null;

            switch (jobType)
            {
                case JobType.Sorting:
                    var sortBuilder = new SortJobBuilder();
                    sortBuilder.Build(JobType.Sorting, scheduleParams.CustomArgument);
                    sortBuilder.WithPriority(scheduleParams.Priority);

                    newJob = sortBuilder.GetInstance();
                break;

                case JobType.ImageProcessingContrast:
                    var contrastBuilder = new ContrastJobBuilder();
                    contrastBuilder.Build(JobType.ImageProcessingContrast, scheduleParams.CustomArgument);
                    contrastBuilder.WithPriority(scheduleParams.Priority);

                    newJob = contrastBuilder.GetInstance();
                break;

                case JobType.ImageProcessingBrightness:
                    var brightnessBuilder = new BrightnessJobBuilder();
                    brightnessBuilder.Build(JobType.ImageProcessingBrightness, scheduleParams.CustomArgument);
                    brightnessBuilder.WithPriority(scheduleParams.Priority);

                    newJob = brightnessBuilder.GetInstance();
                break;
            }

            return newJob;
        }
    }
}