using System;
using PV178.Homeworks.HW06.Model.Enums;
using PV178.Homeworks.HW06.Model.JobParameters;

namespace PV178.Homeworks.HW06.Model
{
    /// <summary>
    /// Represents generic work unit with all related parameters
    /// </summary>
    public class Job
    {
        #region Properties

        /// <summary>
        /// Unique Identifier
        /// </summary>
        public long Id { get; private set; }

        /// <summary>
        /// Type of the work the job performs
        /// </summary>
        public JobType Type { get; }

        /// <summary>
        /// Current state of the job
        /// </summary>
        public JobState State { get; private set; } = JobState.Scheduled;

        /// <summary>
        /// Priority of the job
        /// </summary>
        public JobPriority Priority { get; internal set; } = JobPriority.Normal;

        /// <summary>
        /// Job argument
        /// </summary>
        public JobParameter Argument { get; private set; }

        /// <summary>
        /// Work that job performs
        /// </summary>
        public Action<JobParameter> Work { get; private set; }

        /// <summary>
        /// Job starvation indicator (indicates for how long the job has been waiting in the queue)
        /// </summary>
        public int StarvationLevel { get; private set; } 

        #endregion

        public Job(long id, JobType type)
        {
            Id = id;
            Type = type;
        }

        #region StateModifiers

        /// <summary>
        /// Switches job to "InProgress" state
        /// </summary>
        public void Start()
        {
            State = JobState.InProgress;
        }

        /// <summary>
        /// Switches job to "Cancelled" state
        /// </summary>
        public void Cancel()
        {
            State = JobState.Cancelled;
        }

        /// <summary>
        /// Switches job to "Finished" state
        /// </summary>
        public void Finish()
        {
            State = JobState.Finished;
        }

        #endregion

        #region OneTimeInitializers

        /// <summary>
        /// Initializes job id (if it has not been initialized yet)
        /// </summary>
        /// <param name="id">job id</param>
        public void InitId(long id)
        {
            if (Id == 0)
            {
                Id = id;
            }
        }

        /// <summary>
        /// Initializes job argument (if it has not been initialized yet)
        /// </summary>
        /// <param name="arg">job argument</param>
        public void InitArgument(JobParameter arg)
        {
            if (Argument == null)
            {
                Argument = arg;
            }
        }

        /// <summary>
        /// Initializes job work (if it has not been initialized yet)
        /// </summary>
        /// <param name="work">job work</param>
        public void InitWork(Action<JobParameter> work)
        {
            if (Work == null)
            {
                Work = work;
            }
        }

        #endregion

        #region HelperMethods

        /// <summary>
        /// Increments job starvation level
        /// </summary>
        public void IncreaseStarvationLevel()
        {
            StarvationLevel++;
        }

        public override string ToString()
        {
            return $"ID: {Id}, {State} {Type}, with priority: {Priority} (starvation level: {StarvationLevel})";
        }

        #endregion
    }
}
