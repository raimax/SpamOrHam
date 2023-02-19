using SpamOrHam.Enums;

namespace SpamOrHam.Models
{
    public class ClassificationResponse
    {
        public Classification Classification { get; set; }
        public double ProbabilityForHam { get; set; }
        public double ProbabilityForSpam { get; set; }
        public List<RequestDataPoint> DataPoints { get; set; }
    }
}
