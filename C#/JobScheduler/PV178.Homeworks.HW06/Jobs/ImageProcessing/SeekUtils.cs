using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PV178.Homeworks.HW06.Jobs.ImageProcessing
{
    public class SeekUtils
    {
        public static unsafe void NormalImageSeek(BitmapData imageData, ByteChange byteAction, BackgroundWorker worker, int bytesPerPixel)
        {
            var scan0 = (byte*)imageData.Scan0.ToPointer();

            int percentage = 0;

            worker.ReportProgress(percentage);

            Parallel.For(0, imageData.Height, (int i, ParallelLoopState loopState) =>
            {
                int percentageUpdate = (int)((i / (double)imageData.Height) * 100);
                if (percentage < percentageUpdate)
                {
                    if (percentageUpdate % 10 == 0)
                        worker.ReportProgress(percentageUpdate);
                    percentage = percentageUpdate;
                }

                for (int j = 0; j < imageData.Width; j++)
                {

                    byte* data = scan0 + i * imageData.Stride + j * bytesPerPixel;
                    //Red
                    *data = byteAction(*data);
                    data += 1;
                    //Green
                    *data = byteAction(*data);
                    data += 1;
                    //Blue
                    *data = byteAction(*data);
                }

                if (worker.CancellationPending)
                {
                    loopState.Stop();
                    return;
                }
            });
        }

        public static unsafe void TaskImageSeek(BitmapData imageData, ByteChange byteAction, BackgroundWorker worker, int bytesPerPixel)
        {
            var scan0 = (byte*)imageData.Scan0.ToPointer();

            int percentage = 0;

            worker.ReportProgress(percentage);

            Task[] tasks = new Task[Environment.ProcessorCount];
            
            Action<object> changeBrightnessOnRows = (object obj) =>
            {
                Tuple<int, int> range = (Tuple<int, int>)obj;

                for (int i = range.Item1; i < range.Item2; i++)
                {
                    for (int j = 0; j < imageData.Width; j++)
                    {
                        byte* data = scan0 + i * imageData.Stride + j * bytesPerPixel;
                        //Red
                        *data = byteAction(*data);
                        data += 1;
                        //Green
                        *data = byteAction(*data);
                        data += 1;
                        //Blue
                        *data = byteAction(*data);
                    }
                    if (worker.CancellationPending)
                        return;
                }

                percentage += 100 / Environment.ProcessorCount;
                worker.ReportProgress(percentage);
            };

            int segment = (int)(imageData.Height / (double)Environment.ProcessorCount);
            Parallel.For(0, tasks.Length, (int i) =>
            {
                Tuple<int, int> boundaries = new Tuple<int, int>(i * segment, (i + 1) * segment);
                tasks[i] = new Task(changeBrightnessOnRows, boundaries);
            });

            Parallel.For(0, tasks.Length, (int i) =>
            {
                tasks[i].Start();
            });

            Task.WaitAll(tasks);
        }
    }
}
