namespace PV178.Homeworks.HW06.Model.JobResults.ImageProcessing
{
    /// <summary>
    /// Brightness job result
    /// </summary>
    public class BrightnessResult : JobResult
    {
        public BrightnessResult(long brightnessChange)
            : base($"Changed brightness by {brightnessChange} point(s)") { }
    }
}
