using PV178.Homeworks.HW06.Model.Enums;
using PV178.Homeworks.HW06.Model.JobResults;

namespace PV178.Homeworks.HW06.Model.EventArgs
{
    /// <summary>
    /// Arguments for job done event
    /// </summary>
    public class JobDoneEventArg : JobEventArg
    {
        /// <summary>
        /// The result of the executed job
        /// </summary>
        public JobResult JobResult { get; }

        /// <summary>
        /// Execution time in milliseconds
        /// </summary>
        public long? ExecutionTime { get; }

        public JobDoneEventArg(long jobId, JobType jobType, JobState jobState, JobResult jobResult, long executionTime) : base(jobId, jobType, jobState)
        {
            JobResult = jobResult;
            ExecutionTime = executionTime;
        }
    }
}
