using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PV178.Homeworks.HW06.Content;
using PV178.Homeworks.HW06.Model.JobParameters;
using PV178.Homeworks.HW06.Model.JobParameters.DataSorting;
using PV178.Homeworks.HW06.Model.JobResults.DataSorting;

namespace PV178.Homeworks.HW06.Jobs.DataSorting
{
    public struct ArraysWithPivot
    {
        public int[] Pivot;
        public int[][] Groups;
    }

    public class MagicSort
    {
        private SortParameter sortParameter;
        private int[][] groups;

        public void Sort(JobParameter param)
        {
            try
            {
                sortParameter = (SortParameter) param;

                List<int> results = DoSort();

                if (sortParameter.Worker.CancellationPending)
                {
                    param.Args.Cancel = true;
                    return;
                }

                using (
                    FileStream stream = File.Create(Paths.OutputFolderPath + $"{sortParameter.InputFileName}-sorted.txt")
                )
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    foreach (var result in results)
                    {
                        writer.WriteLine(result);
                    }
                }

                sortParameter.Worker.ReportProgress(100);

                param.Args.Result = new SortResult(((SortParameter) param).InputFileName);
            }
            catch (Exception)
            {
                param.Worker.CancelAsync();
                if (param.Worker.CancellationPending)
                {
                    param.Args.Cancel = true;
                }
            }
        }
            

        private bool CheckIfSorted()
        {
            if (groups == null)
                return true;

            for (int i = 0; i < groups.GetLength(0); i++)
            {
                for (int j = 0; j < groups[i].Length - 1; j++)
                {
                    if (groups[i][j] > groups[i][j + 1])
                        return false;
                }
            }

            return true;
        }

        private List<int> DoSort()
        {
            List<int> entries = LoadResults(Paths.GetResultFullPath(sortParameter.InputFileName));

            FillEntries(entries);

            sortParameter.Worker.ReportProgress(0);
            //step one
            if (SleepSort())
                return null;

            sortParameter.Worker.ReportProgress(50);
            if (CheckIfSorted())
                return MergeResults();
            
            //step two
            SplitAndJoinByPivot();

            if (sortParameter.Worker.CancellationPending)
                return null;

            sortParameter.Worker.ReportProgress(70);

            //step three four five
            List<int> results = MergeResults();

            if (sortParameter.Worker.CancellationPending)
                return null;

            sortParameter.Worker.ReportProgress(90);

            return results;
        }

        private static List<int> LoadResults(string path)
        {
            List<int> output = new List<int>();

            using (var fileStream = File.Open(path, FileMode.Open))
            using (var reader = new StreamReader(fileStream))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if(string.IsNullOrEmpty(line))
                        continue;
                    int result = int.Parse(line);
                    output.Add(result);
                }
            }

            return output;
        }

        private void FillEntries(List<int> results)
        {
            groups = new int[results.Count / 100][];

            for (int i = 0; i < groups.GetLength(0); i++)
            {
                groups[i] = new int[100];
                results.CopyTo(i * 100, groups[i], 0, 100);
            }
        }

        private bool SleepSort()
        {
            Object obj = new Object();

            AutoResetEvent handle = new AutoResetEvent(false);

            for (int i = 0; i < groups.GetLength(0); i++)
            {
                List<int> queue = new List<int>();
                int threadsDone = 0;
                int threadCount = groups[i].Length;

                for (int j = 0; j < groups[i].Length; j++)
                {
                    int number = groups[i][j];

                    Thread th = new Thread(() =>
                    {
                        Thread.Sleep(100 * number);
                        lock (obj)
                        {
                            queue.Add(number);
                            threadsDone++;
                            if (threadsDone == threadCount)
                                handle.Set();
                        }
                    });
                    th.Start();
                }

                handle.WaitOne();

                if (sortParameter.Worker.CancellationPending)
                    return true;
                queue.CopyTo(groups[i]);
                
            }
            return false;
        }

        private void SplitAndJoinByPivot()
        {
            Parallel.For(0, groups.GetLength(0), (int i, ParallelLoopState loopState) =>
            {
                int pivot = groups[i][groups[i].Length / 2];
                int pivotIndex = groups[i].Length / 2;

                int left = 0, right = groups[i].Length - 1;
                while (left <= right)
                {
                    while (groups[i][left] <= pivot) left++;
                    while (groups[i][right] > pivot) right--;

                    if (left <= right)
                    {
                        Swap(ref groups[i][left], ref groups[i][right]);
                        if (left == pivotIndex)
                            pivotIndex = right;
                        else if (right == pivotIndex)
                            pivotIndex = left;
                        left++;
                        right--;
                    }
                }
                //we left pivot behind so we need to move it to the position where left and right indices met
                int distanceFromLeft = Math.Abs(pivotIndex - left);
                int distanceFromRight = Math.Abs(pivotIndex - right);
                int swapIndex = distanceFromLeft < distanceFromRight ? left : right;
                Swap(ref groups[i][pivotIndex], ref groups[i][swapIndex]);

                Parallel.Invoke(
                    () => InsertSort(ref groups[i], 0, pivotIndex), //smaller array A
                    () => InsertSort(ref groups[i], pivotIndex + 1, groups[i].Length) //greater array B
                );
                if (sortParameter.Worker.CancellationPending)
                {
                    loopState.Stop();
                    return;
                }
            });
        }

        private static void InsertSort(ref int[] array, int left, int right)
        {
            if (array == null)
                return;
            if (left < 0 || right > array.Length)
                return;
            for (int i = left; i < right - 1; i++)
            {
                int j = i + 1;

                while (j > 0)
                {
                    if (array[j - 1] > array[j])
                    {
                        Swap(ref array[j - 1], ref array[j]);
                    }
                    j--;
                }
            }
        }

        private List<int> MergeResults()
        {
            List<List<int>> listedGroups = new List<List<int>>();
            for (int i = 0; i < groups.GetLength(0); i++)
            {
                listedGroups.Add(groups[i].ToList());
            }

            while (listedGroups.Count > 1 && !sortParameter.Worker.CancellationPending)
            {
                List<List<int>> partialResult = new List<List<int>>(); 
                Parallel.For(0, listedGroups.Count / 2, (int i,ParallelLoopState loopState) =>
                {
                    partialResult.Add(MergeGroups(listedGroups[2*i],listedGroups[2*i + 1]));
                    if (sortParameter.Worker.CancellationPending)
                    {
                        loopState.Stop();
                        return;
                    }
                });
                listedGroups = partialResult;
            }

            return listedGroups[0];
        }

        private static List<int> MergeGroups(List<int> groupA, List<int> groupB)
        {
            List<int> sortedC1 = null;
            List<int> sortedC2 = null;

            int pivotA = groupA[groupA.Count / 2];
            int pivotIndexB = 0;
            while (groupB[pivotIndexB] <= groupA[pivotA])
            {
                if (pivotIndexB + 1 >= groupB.Count)
                    break;
                pivotIndexB++;
            }
            
            Parallel.Invoke(
                () =>
                {
                    List<int> listA = groupA.GetRange(0, pivotA + 1);
                    List<int> listB = groupB.GetRange(0, pivotIndexB);
                    sortedC1 = Merge(listA, listB);
                },
                () =>
                {
                    List<int> listA = groupA.GetRange(pivotA + 1, groupA.Count - (pivotA + 1));
                    List<int> listB = groupB.GetRange(pivotIndexB, groupB.Count - pivotIndexB);
                    sortedC2 = Merge(listA, listB);
                }
                );
            sortedC1.AddRange(sortedC2);

            return sortedC1;
        }

        private static List<int> Merge(List<int> listA, List<int> listB)
        {
            List<int> mergedList = new List<int>();

            while (listA.Count > 0 && listB.Count > 0)
            {
                if (listA[0] <= listB[0])
                {
                    mergedList.Add(listA[0]);
                    listA.RemoveAt(0);
                }
                else
                {
                    mergedList.Add(listB[0]);
                    listB.RemoveAt(0);
                }
            }

            while (listA.Count > 0)
            {
                mergedList.Add(listA[0]);
                listA.RemoveAt(0);
            }

            while (listB.Count > 0)
            {
                mergedList.Add(listB[0]);
                listB.RemoveAt(0);
            }

            return mergedList;
        }

        private static void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
    }
}
