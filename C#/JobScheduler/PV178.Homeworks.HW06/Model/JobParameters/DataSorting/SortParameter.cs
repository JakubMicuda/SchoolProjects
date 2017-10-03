namespace PV178.Homeworks.HW06.Model.JobParameters.DataSorting
{
    public class SortParameter : JobParameter
    {
        /// <summary>
        /// Input file name
        /// </summary>
        public string InputFileName { get; private set; }

        public void InitInput(string inputFileName)
        {
            if (InputFileName == null)
            {
                InputFileName = inputFileName;
            }
        }
    }
}
