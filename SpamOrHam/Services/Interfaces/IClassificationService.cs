using SpamOrHam.Enums;
using SpamOrHam.Models;

namespace SpamOrHam.Services.Interfaces
{
    public interface IClassificationService
    {
        public Task<Classification> Classify(ClassificationRequest request);
    }
}
