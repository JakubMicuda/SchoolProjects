using System;
using System.ComponentModel;
using System.Drawing;
using PV178.Homeworks.HW06.Model.JobParameters;
using System.Drawing.Imaging;
using PV178.Homeworks.HW06.Content;
using PV178.Homeworks.HW06.Model.Enums;

namespace PV178.Homeworks.HW06.Jobs.ImageProcessing
{
    public delegate void EditTypeAction(JobParameter param, JobType type, SeekAction seekAction);
    public delegate void SeekAction(BitmapData imageData, ByteChange byteAction, BackgroundWorker worker,int bytesPerPixel);

    public class ImageProcess
    {
        public static void EditContrast(JobParameter param) => EditImage(param, JobType.ImageProcessingContrast, SeekUtils.NormalImageSeek);

        public static void EditBrightness(JobParameter param) => EditImage(param, JobType.ImageProcessingBrightness, SeekUtils.TaskImageSeek);

        private static void EditImage(JobParameter param,JobType type, SeekAction imageSeekAction)
        {
            ImageProcessParameter processParameter = new ImageProcessParameter(param, type);

            try
            {
                Bitmap image = processParameter.Image.CreateBitmapDeepCopy();

                var imageData =
                    image.LockBits(
                        new Rectangle(0, 0, processParameter.Image.ImgWidth, processParameter.Image.ImgHeight),
                        ImageLockMode.ReadWrite,
                        processParameter.Image.ImgPixelFormat);

                imageSeekAction(imageData, processParameter.PixelAction, param.Worker, processParameter.Image.BytesPerPixel);

                if (param.Worker.CancellationPending)
                {
                    param.Args.Cancel = true;
                    image.UnlockBits(imageData);
                    return;
                }

                param.Worker.ReportProgress(100);

                image.Save(Paths.GetOutputImageFullName(param.JobId, "jakubsChange", processParameter.Description));

                param.Args.Result = processParameter.Result;

                image.UnlockBits(imageData);
            }catch(Exception)
            {
                param.Worker.CancelAsync();
                if (param.Worker.CancellationPending)
                {
                    param.Args.Cancel = true;
                }
            }
        }
    }
}
