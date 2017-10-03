using System;
using PV178.Homeworks.HW06.Model;
using PV178.Homeworks.HW06.Model.EventArgs;

namespace PV178.Homeworks.HW06.Infrastructure
{
    /// <summary>
    /// Responsible for job execution, note that only single job can be executed at the time
    /// </summary>
    public interface IExecutor
    {
        /// <summary>
        /// Decides, whether new job can be started right now
        /// </summary>
        /// <returns>True if new job can be started</returns>
        bool CanStartNewJob();

        /// <summary>
        /// Starts new job
        /// </summary>
        /// <param name="job">Job to start</param>
        void StartJob(Job job);

        /// <summary>
        /// Cancels currently running job
        /// </summary>
        void CancelCurrentJob();

        /// <summary>
        /// Signals job progress update
        /// </summary>
        event EventHandler<JobProgressUpdateEventArgs> JobUpdate;

        /// <summary>
        /// Signals job completition
        /// </summary>
        event EventHandler<JobDoneEventArg> JobDone;
    }
}