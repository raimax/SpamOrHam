namespace SpamOrHam.Models
{
    public record DatasetResponse
    {
        public int Id { get; init; }
        public double HamCount { get; init; } = 0;
        public double SpamCount { get; init; } = 0;
        public double PriorHamProbability { get; init; }
        public double PriorSpamProbability { get; init; }
        public int DataPointCount { get; init; } = 0;
    }
}
