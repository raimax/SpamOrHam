using SpamOrHam.Enums;

namespace SpamOrHam.Models
{
    public class Dataset
    {
        public int Id { get; set; }
        public List<DataPoint> DataPoints { get; private set; } = new List<DataPoint>();
        public double HamCount { get; private set; } = 0;
        public double SpamCount { get; private set; } = 0;
        public double PriorHamProbability { get; set; }
        public double PriorSpamProbability { get; set; }

        public void AddToCount(Classification classification)
        {
            switch (classification)
            {
                case Classification.Ham:
                    HamCount++;
                    break;
                case Classification.Spam:
                    SpamCount++;
                    break;
                default:
                    break;
            }
        }

        public Dataset AddAlpha(int alpha)
        {
            foreach (var point in DataPoints)
            {
                point.TimesOccurredInHam += alpha;
                point.TimesOccurredInSpam += alpha;
            }

            return this;
        }

        public Dataset CalculatePriorProbabilities()
        {
            PriorHamProbability = HamCount / (HamCount + SpamCount);
            PriorSpamProbability = SpamCount / (SpamCount + HamCount);

            return this;
        }

        public Dataset CalculateDataProbabilities()
        {
            foreach (var point in DataPoints)
            {
                point.CalculateProbabilities(HamCount, SpamCount);
            }

            return this;
        }
    }
}
