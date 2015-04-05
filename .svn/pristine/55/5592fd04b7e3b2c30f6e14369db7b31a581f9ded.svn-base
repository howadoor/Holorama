using System;
using System.Drawing;
using Holorama.Logic.Tools;

namespace Holorama.Logic.Image_Synthesis
{
    public static class BitmapComparator
    {
        public static double GetScore(Bitmap bitmapA, Bitmap bitmapB, int probesCount = 1024)
        {
            if (bitmapA.Size != bitmapB.Size) throw new ArgumentException("Bitmaps must have the same size.");
            double totalDifference = 0.0;
            double maxDiff = 0.0;
            for (int i = 0; i < probesCount; i++)
            {
                var probeX = ColorEx.Random.Next(bitmapA.Width);
                var probeY = ColorEx.Random.Next(bitmapA.Height);
                var pixelA = bitmapA.GetPixel(probeX, probeY);
                var pixelB = bitmapB.GetPixel(probeX, probeY);
                var diff = GetDifferenceARGB(pixelA, pixelB);
                totalDifference += diff;
                if (maxDiff < diff) maxDiff = diff;
            }
            return totalDifference / probesCount;
        }

        private static double GetDifferenceBrightness(Color pixelA, Color pixelB)
        {
            var brightnessDiff = pixelA.GetBrightness() - pixelB.GetBrightness();
            return brightnessDiff*brightnessDiff;
        }

        private static double GetDifferenceARGB(Color pixelA, Color pixelB)
        {
            var da = pixelA.A - pixelB.A;
            var dr = pixelA.R - pixelB.R;
            var dg = pixelA.G - pixelB.G;
            var db = pixelA.B - pixelB.B;
            return da*da + dr*dr + dg*dg +db*db;
        }

        private static double GetDifferenceHSB(Color pixelA, Color pixelB)
        {
            var brightnessDiff = pixelA.GetBrightness() - pixelB.GetBrightness();
            var hueDiff = pixelA.GetHue() - pixelB.GetHue();
            var saturationDiff = pixelA.GetSaturation() - pixelB.GetSaturation();
            return brightnessDiff*brightnessDiff + hueDiff*hueDiff + saturationDiff*saturationDiff;
        }
    }
}