namespace PV178.Homeworks.HW06.Model.JobResults.DataSorting
{
    public class SortResult : JobResult
    {
        public SortResult(string inputFileName) : base($"Sorting of file {inputFileName}.txt was completed") { }
    }
}
