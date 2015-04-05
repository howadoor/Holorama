using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Holorama.Logic.Image_Synthesis.Abstract;
using Holorama.Logic.Tools;
using Ninject;

namespace Holorama.Logic.Image_Synthesis
{
    
    /// <summary>
    /// Optimizes the image synthesis.
    /// </summary>
    public class SynthesisOptimizer
    {
        private const int PopulationCount = 48;

        public Bitmap SourceBitmap
        {
            get { return sourceBitmap; }
            set
            {
                sourceBitmap = value;
                this.sourceBitmapSize = sourceBitmap.Size;
                this.area = new RectangleF(0, 0, sourceBitmap.Width, sourceBitmap.Height);
                InitializeSynthesis();
                var score = BitmapComparator.GetScore(sourceBitmap, sourceBitmap);
            }
        }

        private Size sourceBitmapSize;
        private Bitmap synthesisBackground;

        private Synthesis[] synthesis;
        private RectangleF area;
        private Bitmap sourceBitmap;

        private SynthesisFactory synthesisFactory;

        /// <summary>
        /// Creates an instance and intializes it's members.
        /// </summary>
        /// <param name="synthesisFactory">Factory class used for creating instances of <see cref="ISynthesisItem"/>.</param>
        public SynthesisOptimizer(SynthesisFactory synthesisFactory)
        {
            this.synthesisFactory = synthesisFactory;
        }

        public double AverageScore
        {
            get { return synthesis.Average(s => s.Score); }
        }

        private void InitializeSynthesis()
        {
            synthesis = Enumerable.Range(0, PopulationCount).Select(i => CreateSynthesis()).ToArray();
        }

        private Synthesis CreateSynthesis()
        {
            var newSynthesis = synthesisFactory.Create(area);
            newSynthesis.Score = ComputeScore(newSynthesis);
            return newSynthesis;
        }

        private double ComputeScore(Synthesis newSynthesis)
        {
            using (var synthesizedBitmap = CreateBitmap(newSynthesis))
            {
                return BitmapComparator.GetScore(SourceBitmap, synthesizedBitmap);
            }
        }

        public Synthesis GetBestSynthesis()
        {
            return synthesis.OrderBy(s => s.Score).First();
        }

        public Bitmap CreateBitmap(Synthesis theSynthesis)
        {
            return Synthesizer.SynthesizeBitmap(theSynthesis, synthesisBackground, sourceBitmapSize, area);
        }

        public int RunOptimization(int rounds = 1024)
        {
            var improvements = 0;
            //Parallel.For(0, rounds, i => { if (SingleOptimization()) improvements++; });
            for (int i = 0; i < rounds; i++)
            {
                if (SingleOptimization()) improvements++;
            }
            return improvements;
        }

        private bool SingleOptimization()
        {
            var mutatedSynthesis = CreateMutated(synthesis[ColorEx.Random.Next(synthesis.Length)]);
            var success = false;
            for (int @try = 0; @try < 3; @try++)
            {
                var i = ColorEx.Random.Next(synthesis.Length);
                if (synthesis[i].Score > mutatedSynthesis.Score)
                {
                    synthesis[i] = mutatedSynthesis;
                    success = true;
                }
            }
            return success;
        }

        private Synthesis CreateMutated(Synthesis theSynthesis)
        {
            var mutated = new Synthesis {Items = theSynthesis.Items.Select(i => i.CreateMutated(area)).ToArray()};
            mutated.Score = ComputeScore(mutated);
            return mutated;
        }

        public void SwitchGeneration(bool changeBackground)
        {
            if (changeBackground) synthesisBackground = CreateBitmap(GetBestSynthesis());
            var oldSynthesis = synthesis;
            if (changeBackground)
            {
                foreach (var s in oldSynthesis)
                {
                    s.Score = ComputeScore(s);
                }
            }
            InitializeSynthesis();
            this.synthesis = synthesis.Concat(oldSynthesis).OrderBy(s => s.Score).Take(PopulationCount).ToArray();
        }
    }
}
