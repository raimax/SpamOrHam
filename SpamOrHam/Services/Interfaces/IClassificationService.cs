using SpamOrHam.Models;

namespace SpamOrHam.Services.Interfaces
{
    public interface IClassificationService
    {
        public Task<ClassificationResponse> Classify(ClassificationRequest request);
    }
}
