using PV178.Homeworks.HW06.Model.Enums;

namespace PV178.Homeworks.HW06.Model.JobParameters
{
    /// <summary>
    /// Wrapper class for job scheduling parameters
    /// </summary>
    public class ScheduleParams
    {
        /// <summary>
        /// Job priority
        /// </summary>
        public JobPriority Priority { get; private set; }

        /// <summary>
        /// Job argument
        /// </summary>
        public string CustomArgument { get; private set; }

        /// <summary>
        /// User input data
        /// </summary>
        public string[] ScheduleCommandData { get; private set; }

        public ScheduleParams(JobPriority priority, string customArgument, string[] scheduleCommandData)
        {
            Priority = priority;
            CustomArgument = customArgument;
            ScheduleCommandData = scheduleCommandData;
        }
    }
}
