using System;
using System.Diagnostics;
using PV178.Homeworks.HW06.Model;
using PV178.Homeworks.HW06.Model.Enums;
using PV178.Homeworks.HW06.Model.EventArgs;
using PV178.Homeworks.HW06.Utils;

namespace PV178.Homeworks.HW06.Infrastructure
{
    /// <summary>
    /// Responsible for scheduling jobs
    /// </summary>
    public static class JobScheduler
    {
        /// <summary>
        /// Priority wise job queue
        /// </summary>
        private static readonly IPriorityQueue PriorityQueue = new PriorityQueue();

        /// <summary>
        /// Job executor
        /// </summary>
        private static readonly IExecutor Executor;

        static JobScheduler()
        {
            // TODO initialize Executor

            Executor = new Executor();

            Executor.JobUpdate += Executor_JobUpdate;
            Executor.JobDone += Executor_JobDone;
            AppDomain.CurrentDomain.ProcessExit += ProcessExit;
        }

        /// <summary>
        /// De facto static class destructor used for closing log writer
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">args</param>
        private static void ProcessExit(object sender, EventArgs e)
        {
            Executor.JobUpdate -= Executor_JobUpdate;
            Executor.JobDone -= Executor_JobDone;
            AppDomain.CurrentDomain.ProcessExit -= ProcessExit;
            LogHelper.CloseLogWriter();
        }

        private static void Executor_JobUpdate(object sender, JobProgressUpdateEventArgs e)
        {
            if (e.CurrentJobState == JobState.InProgress)
            {
                Debug.WriteLine($"{e.CurrentJobType} (ID: {e.CurrentJobId}) {e.ProgressPercentage}% done.");                           
            }
        }

        private static void Executor_JobDone(object sender, JobDoneEventArg e)
        {
            string log = null;
            string message;
            switch (e.CurrentJobState)
            {
                case JobState.Finished:
                    var executionTime = e.ExecutionTime.HasValue ? $" in {e.ExecutionTime.Value} ms." : ".";
                    log = $"Finished job: {e.JobResult?.MessageToShow ?? string.Empty}, ID: {e.CurrentJobId}";
                    message = log + executionTime + Environment.NewLine;

                    if (Executor.CanStartNewJob() && !AllJobsHaveFinished())
                    {
                        Job newJob = PriorityQueue.Dequeue();
                        Executor.StartJob(newJob);
                    }


                    break;
                case JobState.Cancelled:
                    message = $"Job: {e.CurrentJobType} (ID: {e.CurrentJobId}) was cancelled by user." + Environment.NewLine;
                    log = $"Canceled job: ID: {e.CurrentJobId}." + Environment.NewLine;
                    if (Executor.CanStartNewJob() && !AllJobsHaveFinished())
                    {
                        Job newJob = PriorityQueue.Dequeue();
                        Executor.StartJob(newJob);
                    }

                    break;
                default:
                    return;
            }
            Debug.WriteLine(message);
            LogHelper.WriteLog(log);          
        }

        /// <summary>
        /// Schedules given job within PriorityQueue
        /// </summary>
        /// <param name="job">Job to schedule</param>
        public static void ScheduleJob(Job job)
        {
            var log = $"Scheduling {job.Type} job (ID: {job.Id}) with {job.Priority} priority.";
            Debug.WriteLine(log);
            LogHelper.WriteLog(log);

            // TODO perform some operation/s here

            PriorityQueue.Enqueue(job);

            if (Executor.CanStartNewJob())
            {
                Job processedJob = PriorityQueue.Dequeue();
                Executor.StartJob(processedJob);
            }
        }

        /// <summary>
        /// Cancels currently running job
        /// </summary>
        public static void CancelCurrentJob()
        {
            Executor.CancelCurrentJob();
        }

        /// <summary>
        /// Check if all scheduled jobs have finished
        /// </summary>
        /// <returns>True if all scheduled jobs have finished, otherwise false</returns>
        public static bool AllJobsHaveFinished()
        {
            return PriorityQueue.GetScheduledJobsCount() == 0 && Executor.CanStartNewJob();
        }
    }
}
