using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Microsoft.Win32.SafeHandles;
using PV178.Homeworks.HW05.Model;

namespace PV178.Homeworks.HW05.Utils
{
    /// <summary>
    /// Utility class for bitmap drawing
    /// </summary>
    public class BitmapDrawer : IDisposable
    {
        private readonly Bitmap bitmap;

        private readonly IList<Pen> pens = new List<Pen>(3);

        public BitmapDrawer(Stream bitmapStream)
        {
            bitmap = new Bitmap(bitmapStream);
            
            pens.Add(new Pen(Color.DodgerBlue, 11));
            pens.Add(new Pen(Color.DeepSkyBlue, 5));
            pens.Add(new Pen(Color.LightSkyBlue, 1));            
        }

        /// <summary>
        /// Drawes entire track to map img
        /// </summary>
        /// <param name="track">Track defined by recorded GPS coordinates</param>
        public void DrawTrack(Track track)
        {
            using (var graphics = Graphics.FromImage(bitmap))
            {
                foreach (var pen in pens)
                {
                    graphics.DrawLines(pen, track.ImagePoints.ToArray());
                }               
            }           
        }

        /// <summary>
        /// Saves bitmap to stream
        /// </summary>
        /// <returns>Bitmap data stream</returns>
        public Stream SaveBitmapToStream()
        {
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream,bitmap.RawFormat);
            return stream;
        }

        public void Dispose()
        {
            bitmap.Dispose();

            foreach (var pen in pens)
            {
                pen.Dispose();
            }
        }
    }
}
