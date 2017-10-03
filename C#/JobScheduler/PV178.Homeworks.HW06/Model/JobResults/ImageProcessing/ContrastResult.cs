namespace PV178.Homeworks.HW06.Model.JobResults.ImageProcessing
{
    /// <summary>
    /// Contrast job result
    /// </summary>
    public class ContrastResult : JobResult
    {
        public ContrastResult(long contrastChange) 
            : base($"Changed contrast by {contrastChange} point(s)") { }
    }
}
