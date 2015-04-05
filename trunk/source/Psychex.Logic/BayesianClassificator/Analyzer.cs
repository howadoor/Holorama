// Type: nBayes.Analyzer
// Assembly: nBayes, Version=0.2.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\Tmp\nBayes.dll

using System;

namespace BayesianTest.BayesianClassificator
{
    public class Analyzer
    {
        private float I;
        private float invI;

        public float Tolerance { get; set; }

        public Analyzer()
        {
            this.Tolerance = 0.05f;
        }

        public CategorizationResult Categorize(Entry item, Index first, Index second)
        {
            foreach (string token in item)
            {
                int tokenCount1 = first.GetTokenCount(token);
                int tokenCount2 = second.GetTokenCount(token);
                float num = this.CalcProbability((float)tokenCount1, (float)first.EntryCount, (float)tokenCount2, (float)second.EntryCount);
                // Console.WriteLine("{0}: [{1}] ({2}-{3}), ({4}-{5})", (object)token, (object)num, (object)tokenCount1, (object)first.EntryCount, (object)tokenCount2, (object)second.EntryCount);
            }
            float num1 = this.CombineProbability();
            if ((double)num1 <= 0.5 - (double)this.Tolerance)
                return CategorizationResult.Second;
            if ((double)num1 >= 0.5 + (double)this.Tolerance)
                return CategorizationResult.First;
            else
                return CategorizationResult.Undetermined;
        }

        private float CalcProbability(float cat1count, float cat1total, float cat2count, float cat2total)
        {
            float num1 = cat1count / cat1total;
            float num2 = cat2count / cat2total;
            float num3 = num1 / (num1 + num2);
            float num4 = 1f;
            float num5 = 0.5f;
            float num6 = cat1count + cat2count;
            float prob = (float)(((double)num4 * (double)num5 + (double)num6 * (double)num3) / ((double)num4 + (double)num6));
            this.LogProbability(prob);
            return prob;
        }

        private void LogProbability(float prob)
        {
            if (float.IsNaN(prob))
                return;
            this.I = (double)this.I == 0.0 ? prob : this.I * prob;
            this.invI = (double)this.invI == 0.0 ? 1f - prob : this.invI * (1f - prob);
        }

        private float CombineProbability()
        {
            return this.I / (this.I + this.invI);
        }
    }
}
