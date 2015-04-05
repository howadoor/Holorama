using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;

namespace Psychex.Logic.Experiments.WordRetrieval
{
    public class ImageCreator
    {
        public Bitmap CreateImage(IEnumerable<string> words, Size bitmapSize, Size wordMargin, out WordPositions wordPositions)
        {
            var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.SmoothingMode = SmoothingMode.HighQuality; 
                CreateImage(words, graphics, bitmapSize, wordMargin, out wordPositions);
            }
            return bitmap;
        }

        private void CreateImage(IEnumerable<string> words, Graphics graphics, Size bitmapSize, Size wordMargin, out WordPositions wordPositions)
        {
            var font = new Font("Cambrium", 12.0f); 
            var random = new Random();
            graphics.FillRectangle(Brushes.Transparent, 0, 0, bitmapSize.Width, bitmapSize.Height);
            var wordsToPlaceIn = words.ToList();
            var backgroundRectangles = new List<RectangleF>();
            wordPositions = new WordPositions();
            for (; wordsToPlaceIn.Count > 0;)
            {
                var wordIndex = random.Next(wordsToPlaceIn.Count);
                var word = wordsToPlaceIn [wordIndex];
                wordsToPlaceIn.RemoveAt(wordIndex);
                var wordSize = graphics.MeasureString(word, font);
                for (var retry = 0; ; retry++)
                {
                    var posX = (float) (random.NextDouble()*(bitmapSize.Width - wordSize.Width - wordMargin.Width*2)) + wordMargin.Width;
                    var posY = (float) (random.NextDouble()*(bitmapSize.Height - wordSize.Height - wordMargin.Height*2)) + wordMargin.Height;
                    var background = new RectangleF(posX - wordMargin.Width, posY - wordMargin.Height, wordSize.Width + wordMargin.Width*2, wordSize.Height + wordMargin.Height*2);
                    if (!backgroundRectangles.Any(r => r.IntersectsWith(background)))
                    {
                        backgroundRectangles.Add(background);
                        graphics.FillRectangle(Brushes.Black, background);
                        graphics.DrawString(word, font, Brushes.White, posX, posY);
                        wordPositions[word] = new RectangleF(posX, posY, wordSize.Width, wordSize.Height);
                        break;
                    }
                    if (retry > 16384) throw new InvalidOperationException("Max retries of word placement reached");
                }
            }
        }
       
        public Bitmap CreateImage( WordPositions wordPositions, Size bitmapSize)
        {
            var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                CreateImage(graphics, wordPositions);
            }
            return bitmap;
        }

        public void CreateImage(Graphics graphics, WordPositions wordPositions)
        {
            var font = new Font("Cambrium", 12.0f);
            var random = new Random();
            foreach (var wordPosition in wordPositions)
            {
                var wordRectangle = wordPosition.Value;
                graphics.DrawString(wordPosition.Key, font, Brushes.Black, wordRectangle.Left, wordRectangle.Top);
            }
        }
    }
}
