using PV178.Homeworks.HW06.Model;

namespace PV178.Homeworks.HW06.Infrastructure
{
    /// <summary>
    /// Priority wise queue for scheduling jobs
    /// </summary>
    public interface IPriorityQueue
    {
        /// <summary>
        /// Adds job to queue
        /// </summary>
        /// <param name="job">Job to enqueue</param>
        void Enqueue(Job job);

        /// <summary>
        /// Picks next job from the queue
        /// </summary>
        /// <returns>Next job if queue is not empty, otherwise returns null</returns>
        Job Dequeue();

        /// <summary>
        /// Gets number of currently scheduled jobs within the queue
        /// </summary>
        /// <returns>Number of currently scheduled jobs</returns>
        int GetScheduledJobsCount();
    }
}