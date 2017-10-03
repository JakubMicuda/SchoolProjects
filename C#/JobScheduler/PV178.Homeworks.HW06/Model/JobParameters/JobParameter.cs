using System.ComponentModel;

namespace PV178.Homeworks.HW06.Model.JobParameters
{
    /// <summary>
    /// Base class representing job parameter
    /// </summary>
    public abstract class JobParameter
    {
        /// <summary>
        /// Job identifier
        /// </summary>
        public long JobId { get; private set; }

        /// <summary>
        /// DoWork event arguments (used for job result storage)
        /// </summary>
        public DoWorkEventArgs Args { get; private set; }

        /// <summary>
        /// BackgroundWorker instance
        /// </summary>
        public BackgroundWorker Worker { get; private set; }

        /// <summary>
        /// Job id initialization
        /// </summary>
        /// <param name="jobId">Job identifier</param>
        public void InitId(long jobId)
        {
            if (JobId == 0)
            {
                JobId = jobId;
            }         
        }

        /// <summary>
        /// Job worker instance initialization
        /// </summary>
        /// <param name="worker">BackgroundWorker instance</param>
        public void InitWorker(BackgroundWorker worker)
        {
            if (Worker == null)
            {
                Worker = worker;
            }
        }

        /// <summary>
        /// DoWork event arguments initialization
        /// </summary>
        /// <param name="args">DoWork event arguments</param>
        public void InitDoWorkArgs(DoWorkEventArgs args)
        {
            Args = args;
        }
    }
}
