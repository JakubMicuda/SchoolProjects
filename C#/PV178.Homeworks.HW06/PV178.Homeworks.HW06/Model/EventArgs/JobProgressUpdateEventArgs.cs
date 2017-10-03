using PV178.Homeworks.HW06.Model.Enums;

namespace PV178.Homeworks.HW06.Model.EventArgs
{
    /// <summary>
    /// Arguments for progress update event
    /// </summary>
    public class JobProgressUpdateEventArgs : JobEventArg
    {
        /// <summary>
        /// Current progress percentage
        /// </summary>
        public int ProgressPercentage { get; }

        public JobProgressUpdateEventArgs(long jobId, JobType jobType, JobState jobState, int progressPercentage) : base(jobId, jobType, jobState)
        {            
            ProgressPercentage = progressPercentage;
        }
    }
}
