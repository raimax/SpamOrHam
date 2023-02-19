using Microsoft.EntityFrameworkCore;
using SpamOrHam.Enums;
using SpamOrHam.Models;
using SpamOrHam.Services.Interfaces;
using SpamOrHam.SqlServer;

namespace SpamOrHam.Services
{
    public class ClassificationService : IClassificationService
    {
        private Dataset? _dataset;
        private readonly IServiceScopeFactory _scopeFactory;

        public ClassificationService(IServiceScopeFactory scopeFactory)
        {
            this._scopeFactory = scopeFactory;
        }

        public async Task<ClassificationResponse> Classify(ClassificationRequest request)
        {
            if (_dataset is null)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                    _dataset = await context.Datasets
                        .Include(x => x.DataPoints)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();

                    await context.DisposeAsync();
                }
            }

            if (_dataset is null)
            {
                throw new Exception("Dataset is not found");
            }

            List<RequestDataPoint> requestList = new();

            foreach (var word in request.Content.Trim().Split(" ").ToList())
            {
                var wordInDataset = _dataset.DataPoints.FirstOrDefault(x => x.Word.ToLower() == word.ToLower());

                if (wordInDataset is null)
                {
                    continue;
                }

                var existingPoint = requestList.FirstOrDefault(x => x.Word.ToLower() == word.ToLower());

                if (existingPoint is null)
                {
                    requestList.Add(new RequestDataPoint
                    {
                        Word = word,
                        Count = 1.0,
                        HamProbability = wordInDataset.HamProbability,
                        SpamProbability = wordInDataset.SpamProbability,
                    });

                    continue;
                }

                var existingPointIndex = requestList.FindIndex(x => x.Word.ToLower() == word.ToLower());

                requestList[existingPointIndex].Count++;
            }

            var probabilityForHam = _dataset.PriorHamProbability;
            var probabilityForSpam = _dataset.PriorSpamProbability;

            foreach (var point in requestList)
            {
                if (point.Count > 0)
                {
                    probabilityForHam *= Math.Pow(point.HamProbability, point.Count);
                    probabilityForSpam *= Math.Pow(point.SpamProbability, point.Count);
                }
            }

            return new ClassificationResponse
            {
                Classification = probabilityForHam > probabilityForSpam ? Classification.Ham : Classification.Spam,
                DataPoints = requestList,
                ProbabilityForHam = probabilityForHam,
                ProbabilityForSpam = probabilityForSpam
            };
        }
    }
}
