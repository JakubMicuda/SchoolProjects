using System;
using PV178.Homeworks.HW06.Jobs.DataSorting;
using PV178.Homeworks.HW06.Model.JobParameters;
using PV178.Homeworks.HW06.Model.JobParameters.DataSorting;
using PV178.Homeworks.HW06.Model.JobResults;
using PV178.Homeworks.HW06.Model.JobResults.DataSorting;
using PV178.Homeworks.HW06.Utils.JobBuilders;

namespace PV178.Homeworks.HW06.Utils.JobBuilders.DataSorting
{
    public class SortJobBuilder : JobBuilder<SortParameter, SortResult>
    {
        protected override void SetArgumentCore(string customParameter)
        {
            SortParameter arg = (SortParameter)Job.Argument;
            arg.InitInput(customParameter);
        }

        protected override void SetWork()
        {
            MagicSort magicSort = new MagicSort();
            Job.InitWork(magicSort.Sort);
        }
    }
}
