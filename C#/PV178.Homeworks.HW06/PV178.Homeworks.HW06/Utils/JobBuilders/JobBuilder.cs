using PV178.Homeworks.HW06.Model;
using PV178.Homeworks.HW06.Model.Enums;
using PV178.Homeworks.HW06.Model.JobParameters;
using PV178.Homeworks.HW06.Model.JobResults;

namespace PV178.Homeworks.HW06.Utils.JobBuilders
{
    /// <summary>
    /// Base class for building jobs of various types
    /// </summary>
    /// <typeparam name="T1">Type of job parameter</typeparam>
    /// <typeparam name="T2">Type of job result</typeparam>
    public abstract class JobBuilder<T1, T2> where T1 : JobParameter, new() where T2 : JobResult
    {
        /// <summary>
        /// Constructed job
        /// </summary>
        protected Job Job;

        /// <summary>
        /// Job accessor
        /// </summary>
        /// <returns></returns>
        public Job GetInstance() => Job;

        /// <summary>
        /// Build job instance
        /// </summary>
        /// <param name="type">Type of job to build</param>
        /// <param name="customParameter">Job argument (optional)</param>
        /// <returns>This builder</returns>
        public JobBuilder<T1, T2> Build(JobType type, string customParameter = null)
        {
            var id = JobIdAssigner.AssignId();
            Job = new Job(id, type);
            SetArgument(id, customParameter);
            SetWork(); 
            return this;          
        }

        /// <summary>
        /// Assigns specified priority to constructed job
        /// </summary>
        /// <param name="priority">Priority to set</param>
        /// <returns>This builder</returns>
        public JobBuilder<T1, T2> WithPriority(JobPriority priority)
        {
            Job.Priority = priority;
            return this;
        }

        /// <summary>
        /// Sets job argument
        /// </summary>
        /// <param name="jobId">job ID</param>
        /// <param name="customParameter">job parameter</param>
        private void SetArgument(long jobId, string customParameter)
        {
            var parameter = new T1();
            parameter.InitId(jobId);
            Job.InitArgument(parameter);
            SetArgumentCore(customParameter);
        }

        /// <summary>
        /// Assigns job parameter
        /// </summary>
        /// <param name="customParameter"></param>
        protected abstract void SetArgumentCore(string customParameter);

        /// <summary>
        /// Assigns method to Work delegate from Job class
        /// </summary>
        protected abstract void SetWork();
    }
}
