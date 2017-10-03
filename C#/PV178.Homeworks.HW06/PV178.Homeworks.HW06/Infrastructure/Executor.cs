using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PV178.Homeworks.HW06.Model;
using PV178.Homeworks.HW06.Model.Enums;
using PV178.Homeworks.HW06.Model.EventArgs;
using PV178.Homeworks.HW06.Model.JobResults.DataSorting;
using PV178.Homeworks.HW06.Model.JobResults;

namespace PV178.Homeworks.HW06.Infrastructure
{
    public class Executor : IExecutor
    {
        private Job currentJob = null;
        private Stopwatch time = new Stopwatch();
        private readonly BackgroundWorker worker = new BackgroundWorker();

        public Executor()
        {
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
        }

        public bool CanStartNewJob()
        {
            if (currentJob == null)
                return true;

            return !worker.IsBusy;
        }

        public void StartJob(Job job)
        {

            currentJob = job;

            currentJob.Argument.InitWorker(worker);
            currentJob.Start();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            time.Start();
            worker.RunWorkerAsync();
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            JobProgressUpdateEventArgs jobArgs = new JobProgressUpdateEventArgs(currentJob.Id,currentJob.Type,currentJob.State,e.ProgressPercentage);

            JobUpdate?.Invoke(sender, jobArgs);
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            currentJob.Argument.InitDoWorkArgs(e);
            currentJob.Work(currentJob.Argument);
            time.Start();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            time.Stop();

            JobResult result = null;
            if (!e.Cancelled && e.Error == null)
            {
                currentJob.Finish();
                result = (JobResult) e.Result;
            }
            else
                currentJob.Cancel();


            JobDoneEventArg jobArg = new JobDoneEventArg(currentJob.Id, currentJob.Type, currentJob.State, result, time.ElapsedMilliseconds);

            JobDone?.Invoke(sender, jobArg);

            time.Reset();
        }

        public void CancelCurrentJob()
        {
            if(CanStartNewJob())
                return;

            worker.CancelAsync();
            if (worker.CancellationPending)
            {
                currentJob.Argument.Args.Cancel = true;
                currentJob.Cancel();
            }
        }

        public event EventHandler<JobProgressUpdateEventArgs> JobUpdate;
        public event EventHandler<JobDoneEventArg> JobDone;
    }
}
