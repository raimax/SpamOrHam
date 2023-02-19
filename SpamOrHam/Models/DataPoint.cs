using SpamOrHam.Enums;

namespace SpamOrHam.Models
{
    public class DataPoint
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public double TimesOccurredInHam { get; set; }
        public double TimesOccurredInSpam { get; set; }
        public double HamProbability { get; set; }
        public double SpamProbability { get; set; }
        public string LastRecordContent { get; set; }
        public int DatasetId { get; set; }
        public Dataset Dataset { get; set; }


        public void AddToCount(Classification classification)
        {
            switch (classification)
            {
                case Classification.Ham:
                    TimesOccurredInHam++;
                    break;
                case Classification.Spam:
                    TimesOccurredInSpam++;
                    break;
                default:
                    break;
            }
        }

        public void CalculateProbabilities(double hamCount, double spamCount)
        {
            HamProbability = TimesOccurredInHam / hamCount;
            SpamProbability = TimesOccurredInSpam / spamCount;
        }
    }
}
