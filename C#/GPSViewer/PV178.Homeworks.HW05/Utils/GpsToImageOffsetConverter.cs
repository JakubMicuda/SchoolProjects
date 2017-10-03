using System;
using System.Drawing;
using PV178.Homeworks.HW05.Model;

namespace PV178.Homeworks.HW05.Utils
{
    /// <summary>
    /// Converts GPS coordinates to image width and height offsets (from top left corner) 
    /// </summary>
    public class GpsToImageOffsetConverter
    {
        #region ReadonlyFields
        private const int Zoom = 14;
        private const int MapTileSize = 256;
        private readonly Point topLeftPixel;
        private static readonly double TotalPixelSize = MapTileSize * Math.Pow(2d, Zoom);
        private static readonly double PixelsToDegreesRatio = TotalPixelSize / 360d;
        private static readonly double PixelsToRadiansRatio = TotalPixelSize / (2 * Math.PI); 

        /// <summary>
        /// GPS coordinates of the map image top left corner
        /// </summary>
        //private static readonly GpsCoordinates TopLeft = new GpsCoordinates(49.4235392, 16.5788069); 
        private static readonly GpsCoordinates TopLeft = new GpsCoordinates(49.4301964, 16.5512897);
        #endregion

        public GpsToImageOffsetConverter()
        {
            /* computes top left corner pixel values in order to 
             calculate values for image offset of GPS coordinates */
            topLeftPixel = ConvertGpsToImageOffset(TopLeft);
        }
       /// <summary>
        /// Converts GPS coordinates to point, which represents 
        /// width and height pixel offset from top left corner of the map image 
        /// </summary>
        /// <param name="coords">GPS coordinates to convert</param>
        /// <returns>Width and height pixel offset wrapped within image</returns>
        public Point ConvertGpsToImageOffset(GpsCoordinates coords)
        {
            var x = ComputeWidthOffset(coords.Longitude);
            var y = ComputeHeightOffset(coords.Latitude);
            return new Point(x, y);
        }

        #region ConverterHelperMethods

        /// <summary>
        /// Converts GPS longitude to width offset within image (from top left corner)
        /// Taken from https://code.google.com/archive/p/geographical-dot-net/
        /// </summary>
        /// <param name="longitude">GPS longitude to convert to width offset</param>
        /// <returns>width offset (in pixels)</returns>
        private int ComputeWidthOffset(double longitude)
        {
            var x = Math.Round((TotalPixelSize / 2d) + (longitude * PixelsToDegreesRatio));
            return (int)Math.Round(x, 0) - topLeftPixel.X;
        }

        /// <summary>
        /// Converts GPS latitude to height offset within image (from top left corner)
        /// Taken from https://code.google.com/archive/p/geographical-dot-net/
        /// </summary>
        /// <param name="latitude">GPS latitude to convert to width offset</param>
        /// <returns>height offset (in pixels)</returns>
        private int ComputeHeightOffset(double latitude)
        {
            var normalizedSin = Math.Min(Math.Max(Math.Sin(latitude * (Math.PI / 180d)), -0.99999d), 0.99999d);
            var y = Math.Round(TotalPixelSize / 2d + 0.5d * Math.Log((1d + normalizedSin) / (1d - normalizedSin)) * (-1) * PixelsToRadiansRatio);
            return (int)Math.Round(y, 0) - topLeftPixel.Y;
        }


        /* Copyright 2016 geographical-dot-net

            Licensed under the Apache License, Version 2.0 (the "License");
            you may not use this file except in compliance with the License.
            You may obtain a copy of the License at

                http://www.apache.org/licenses/LICENSE-2.0

            Unless required by applicable law or agreed to in writing, software
            distributed under the License is distributed on an "AS IS" BASIS,
            WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
            See the License for the specific language governing permissions and
            limitations under the License.
        */

        #endregion
    }
}