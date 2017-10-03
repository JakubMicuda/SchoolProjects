using System;
using System.Collections;
using PV178.Homeworks.HW06.Model;
using System.Collections.Generic;
using System.Linq;
using PV178.Homeworks.HW06.Model.Enums;

namespace PV178.Homeworks.HW06.Infrastructure
{
    public class PriorityQueue : IPriorityQueue
    {
        private const int AboveAverageMaxStarvationLevel = 4;
        private const int NormalMaxStarvationLevel = 8;
        private const int BelowAverageMaxStarvationLevel = 12;
        private readonly Object mylock = new Object();

        private readonly List<Job> priorityQueue = new List<Job>();

        private class PriorityComparer : IComparer<Job>
        {
            private int AboveAverageMaxStarvationLevel;
            private int NormalMaxStarvationLevel;
            private int BelowAverageMaxStarvationLevel;

            public PriorityComparer(int aboveAvgMax, int normalMax, int belowMax)
            {
                AboveAverageMaxStarvationLevel = aboveAvgMax;
                NormalMaxStarvationLevel = normalMax;
                BelowAverageMaxStarvationLevel = belowMax;
            }

            public int Compare(Job x, Job y)
            {
                if (IsStarved(x) && IsStarved(y))
                {
                    if (x.StarvationLevel > y.StarvationLevel)
                        return -1;
                    if (x.StarvationLevel == y.StarvationLevel)
                        return 0;
                    return 1;
                }

                if (IsStarved(x) && !IsStarved(y))
                {
                    return -1;
                }

                if (!IsStarved(x) && IsStarved(y))
                {
                    return 1;
                }

                if ((int) x.Priority == (int) y.Priority)
                    return 0;
                if ((int)x.Priority < (int)y.Priority)
                    return -1;
                return 1;
            }

            private bool IsStarved(Job job)
            {
                bool isStarved = false;

                switch (job.Priority)
                {
                    case JobPriority.AboveAverage:
                        isStarved = job.StarvationLevel >= AboveAverageMaxStarvationLevel;
                        break;

                    case JobPriority.Normal:
                        isStarved = job.StarvationLevel >= NormalMaxStarvationLevel;
                        break;

                    case JobPriority.BelowAverage:
                        isStarved = job.StarvationLevel >= BelowAverageMaxStarvationLevel;
                        break;
                }

                return isStarved;
            }
        }

        readonly PriorityComparer comparer = new PriorityComparer(AboveAverageMaxStarvationLevel, NormalMaxStarvationLevel, BelowAverageMaxStarvationLevel);

        public void Enqueue(Job job)
        {
            if (job == null)
                return;

            lock (mylock)
            {
                AddStarvation();
                priorityQueue.Add(job);
                priorityQueue.Sort(comparer);
            }
        }

        private void AddStarvation()
        {
            foreach (Job job in priorityQueue)
            {
                job.IncreaseStarvationLevel();
            }
        }

        public Job Dequeue()
        {
            lock (mylock)
            {
                if (priorityQueue.Count == 0)
                    return null;
                Job head = priorityQueue[0];
                priorityQueue.RemoveAt(0);

                return head;
            }
        }

        public int GetScheduledJobsCount()
        {
            return priorityQueue.Count;
        }
    }
}
