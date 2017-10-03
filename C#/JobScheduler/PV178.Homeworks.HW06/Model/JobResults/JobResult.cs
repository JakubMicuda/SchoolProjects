namespace PV178.Homeworks.HW06.Model.JobResults
{
    /// <summary>
    /// Common base class for all job results
    /// </summary>
    public abstract class JobResult
    {
        /// <summary>
        /// Message describing the job result
        /// </summary>
        public string MessageToShow { get; }

        protected JobResult(string message)
        {
            MessageToShow = message;
        }
    }
}
