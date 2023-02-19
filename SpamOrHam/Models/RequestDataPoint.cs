namespace SpamOrHam.Models
{
    public class RequestDataPoint
    {
        public string Word { get; set; }
        public double Count { get; set; }
        public double HamProbability { get; set; }
        public double SpamProbability { get; set; }
    }
}
