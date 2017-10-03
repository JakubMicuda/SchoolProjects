using PV178.Homeworks.HW06.Model.Enums;

namespace PV178.Homeworks.HW06.Model.EventArgs
{
    /// <summary>
    /// Base class for job events
    /// </summary>
    public abstract class JobEventArg
    {
        /// <summary>
        /// Id of the current job
        /// </summary>
        public long CurrentJobId { get; }

        /// <summary>
        /// Type of the current job
        /// </summary>
        public JobType CurrentJobType { get; }

        /// <summary>
        /// State of the current job
        /// </summary>
        public JobState CurrentJobState { get; }

        protected JobEventArg(long jobId, JobType type, JobState jobState)
        {
            CurrentJobId = jobId;
            CurrentJobType = type;
            CurrentJobState = jobState;
        }
    }
}
