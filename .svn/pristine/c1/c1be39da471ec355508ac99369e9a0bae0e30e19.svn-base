using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace Psychex.Logic.Helpers
{
    public class KohonenMap
    {
        private Random random = new Random();
        
        public void CalculateMap<TItem>(Dictionary<TItem, double[]> sourceVectors)
        {
            var map = CreateMap(sourceVectors.Values);
            PerformCalculation(sourceVectors.Values.ToArray(), map);
            // DrawMap(map, sourceVectors);
            SaveMapHtml(map, sourceVectors);
        }

        private void SaveMapHtml<TItem>(double[,][] map, Dictionary<TItem, double[]> sourceVectors)
        {
            var document = new HtmlDocument();
            var node = HtmlNode.CreateNode("<html><head><link rel=\"stylesheet\" href=\"kohonen.css\" type=\"text/css\" /></head><body></body></html>");
            document.DocumentNode.AppendChild(node);
            document.DocumentNode.Descendants("body").First().AppendChild(CreateMapHtml(map, sourceVectors));
            document.Save(@"c:\kohonen.html", Encoding.UTF8);
        }

        private HtmlNode CreateMapHtml<TItem>(double[,][] map, Dictionary<TItem, double[]> sourceVectors)
        {
            var mapNode = HtmlNode.CreateNode("<div class=\"map\"></div>");
            var lists = new Dictionary<Point, List<TItem>>();
            foreach (var sourceVector in sourceVectors)
            {
                var bestMatching = FindBestMatchingVector(sourceVector.Value, map);
                List<TItem> list;
                if (!lists.TryGetValue(bestMatching, out list))
                {
                    lists [bestMatching] = list = new List<TItem>();    
                }
                list.Add(sourceVector.Key);
            }
            foreach (var listItem in lists)
            {
                mapNode.AppendChild(CreatePointHtml(listItem.Key, listItem.Value, map));
            }
            return mapNode;
        }

        private HtmlNode CreatePointHtml<TItem>(Point point, List<TItem> items, double[,][] map)
        {
            var lx = map.GetLength(0);
            var ly = map.GetLength(1);
            var x = 100.0*point.X/lx;
            var y = 100.0 * point.Y / ly;
            int magnitude = (int) Math.Log10(items.Count);
            var pointNode = HtmlNode.CreateNode(string.Format("<div class=\"point magnitude-{0}\" style=\"left:{2}%; top:{3}%\"><span class=\"count\">{1}</span><ul></ul></div>", magnitude, items.Count, x.ToString("0.0", CultureInfo.InvariantCulture), y.ToString("0.0", CultureInfo.InvariantCulture)));
            var listNode = pointNode.Descendants("ul").First();
            foreach (var item in items.OrderBy(i => i.ToString()))
            {
                listNode.AppendChild(HtmlNode.CreateNode(string.Format("<li>{0}</li>", item)));
            }
            return pointNode;
        }

        private void PerformCalculation(double[][] sourceVectors, double[,][] map)
        {
            var lx = map.GetLength(0);
            var ly = map.GetLength(1);
            var iterations = sourceVectors.Length * 32;
            for (int i = 0; i < iterations; i++)
            {
                var randomVector = sourceVectors[random.Next(sourceVectors.Length)];
                var bestMatching = FindBestMatchingVector(randomVector, map);
                AdaptVectors(bestMatching, randomVector, map, (iterations - i));
            }
        }

        private void AdaptVectors(Point bestMatching, double[] sourceVector, double[,][] map, double coef)
        {
            var lx = map.GetLength(0);
            var ly = map.GetLength(1);
            for (int x = 0; x < lx; x++)
            {
                for (int y = 0; y < ly; y++)
                {
                    var distance = (x - bestMatching.X)*(x - bestMatching.X) + (y - bestMatching.Y)*(y - bestMatching.Y);
                    var factor = coef/(1.0 + distance);
                    AdaptVector(map[x,y], sourceVector, factor);
                }
            }
        }

        private void AdaptVector(double[] adaptedVector, double[] sourceVector, double factor)
        {
            for (var i = 0; i < adaptedVector.Length; i++)
            {
                adaptedVector[i] = (adaptedVector[i] + sourceVector[i] * factor) / (1 + factor);
            }
        }

        private void DrawMap<TItem>(double[,][] map, Dictionary<TItem, double[]> sourceVectors)
        {
            Bitmap bitmap;
            using (var graphics = CreateMapGraphics(map, out bitmap))
            {
                DrawMap(graphics, map, sourceVectors);
            }
            bitmap.Save(@"c:\kohonen.png");
            bitmap.Dispose();
            var lx = map.GetLength(0);
            var ly = map.GetLength(1);
        }

        private void DrawMap<TItem>(Graphics graphics, double[,][] map, Dictionary<TItem, double[]> sourceVectors)
        {
            var color = Color.Black;
            var width = (float) 0.01;
            var pen = new Pen(color, width);
            var font = new Font("Calibri", (float)0.30);
            var stringFormat = new StringFormat {Alignment = StringAlignment.Center};
            foreach (var sourceVector in sourceVectors)
            {
                var bestMatching = FindBestMatchingVector(sourceVector.Value, map);
                var center = new PointF(bestMatching.X, bestMatching.Y);
                var randomAngle = random.NextDouble()*Math.PI*2;
                var textCenter = new PointF((float) (center.X + 0.5*Math.Cos(randomAngle)), (float) (center.Y + 0.5*Math.Sin(randomAngle)));
                graphics.DrawLine(pen, center, textCenter);
                graphics.DrawCircle(center, 0.08f, pen, new SolidBrush(Color.White));
                graphics.DrawString(sourceVector.Key.ToString(), font, new SolidBrush(Color.Black), textCenter.X, (float)(textCenter.Y + (textCenter.Y > center.Y ? -0.05f : -0.4f)), stringFormat);
                // var bestCoord = FindBestMatchingCoordinates(sourceVector.Value, bestMatching, map);
                // graphics.DrawLine(pen, center, bestCoord);
            }
        }

        private PointF FindBestMatchingCoordinates(double[] sourceVector, Point bestMatching, double[,][] map)
        {
            var lx = map.GetLength(0);
            var ly = map.GetLength(1);
            double bestX = 0, bestY = 0, weightSumm = 0;
            for (int x = 0; x < lx; x++)
            {
                for (int y = 0; y < ly; y++)
                {
                    var distance = (x - bestMatching.X) * (x - bestMatching.X) + (y - bestMatching.Y) * (y - bestMatching.Y);
                    var difference = ComputeDifference(sourceVector, map[x, y]);
                    var factor = 1.0/(difference * difference * (1 + distance));
                    bestX += x*factor;
                    bestY += y*factor;
                    weightSumm += factor;
                }
            }
            return new PointF((float) (bestX / weightSumm), (float) (bestY / weightSumm));
        }

        private Graphics CreateMapGraphics(double[,][] map, out Bitmap bitmap)
        {
            var lx = map.GetLength(0);
            var ly = map.GetLength(1);
            var area = new RectangleF((float) (lx/-10.0), (float) (ly/-10.0), (float) (lx*1.2), (float) (ly*1.2));
            var bitmapHeight = 2048;
            var bitmapWidth = (int) (bitmapHeight*area.Width/area.Height + 0.5);
            bitmap = new Bitmap(bitmapWidth, bitmapHeight);
            var graphics = Graphics.FromImage(bitmap);
            graphics.Transform = new Matrix(area, new PointF[] { new PointF(0, 0), new PointF(bitmapWidth, 0), new PointF(0, bitmapHeight) });
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit; 
            return graphics;
        }

        private Point FindBestMatchingVector(double[] sourceVector, double[,][] map)
        {
            var lx = map.GetLength(0);
            var ly = map.GetLength(1);
            var minDiff = double.MaxValue;
            Point best = new Point();
            for (int x = 0; x < lx; x++)
            {
                for (int y = 0; y < ly; y++)
                {
                    var difference = ComputeDifference(sourceVector, map[x, y]);
                    if (difference < minDiff)
                    {
                        minDiff = difference;
                        best = new Point(x, y);
                    }
                }
            }
            return best;
        }

        private double ComputeDifference(double[] v1, double[] v2)
        {
            double summ = v1.Select((t, i) => (t - v2[i])*(t - v2[i])).Sum();
            return Math.Sqrt(summ);
        }

        private double[,][] CreateMap(IEnumerable<double[]> sourceVectors)
        {
            var average = ComputeAverages(sourceVectors);
            var variances = ComputeVariances(sourceVectors, average);
            var map = new double[64,48][];
            var lx = map.GetLength(0);
            var ly = map.GetLength(1);
            for (int x = 0; x < lx; x++)
            {
                for (int y = 0; y < ly; y++)
                {
                    map[x, y] = ComputeRandomVector(average, variances);
                }
            }
            return map;
        }

        private double[] ComputeRandomVector(double[] average, double[] variances)
        {
            double[] vector = new double[average.Length];
            for (var i = 0; i < vector.Length; i++)
            {
                vector[i] = average[i] + (random.NextDouble() - 0.5)*variances[i]*10.0;
            }
            return vector;
        }

        /// <summary>
        /// Computes variance (CZ: rozptyl, http://cs.wikipedia.org/wiki/Rozptyl_(statistika))
        /// </summary>
        /// <param name="sourceVectors"></param>
        /// <param name="average"></param>
        /// <returns></returns>
        private double[] ComputeVariances(IEnumerable<double[]> sourceVectors, double[] average)
        {
            double[] summ = null;
            int count = 0;
            foreach (var sourceVector in sourceVectors)
            {
                if (count == 0)
                {
                    summ = new double[sourceVector.Length];
                }
                for (var i = 0; i < summ.Length; i++) summ[i] += (average[i] - sourceVector[i]) * (average[i] - sourceVector[i]);
                count++;
            }
            for (var i = 0; i < summ.Length; i++) summ[i] = Math.Sqrt(summ[i] / count);
            return summ;
        }

        /// <summary>
        /// Computes averages of all vectors
        /// </summary>
        /// <param name="sourceVectors"></param>
        private double[] ComputeAverages(IEnumerable<double[]> sourceVectors)
        {
            double[] summ = null;
            int count = 0;
            foreach (var sourceVector in sourceVectors)
            {
                if (count == 0)
                {
                    summ = new double[sourceVector.Length];
                }
                for (var i = 0; i < summ.Length; i++) summ[i] += sourceVector[i];
                count++;
            }
            for (var i = 0; i < summ.Length; i++) summ[i] = summ[i] / count;
            return summ;
        }
    }
}