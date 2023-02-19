using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using SpamOrHam.Enums;
using SpamOrHam.Models;
using System.Globalization;

namespace SpamOrHam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassificationController : ControllerBase
    {
        public ClassificationController()
        {
        }

        [HttpPost("classify")]
        public IActionResult Classify([FromBody] ClassificationRequest request)
        {
            List<CsvDataRow> records = new();
            var dataset = new Dataset();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Mode = CsvMode.NoEscape,
                MissingFieldFound = null
            };

            using (var reader = new StreamReader("data\\spam_or_not_spam.csv"))
            using (var csv = new CsvReader(reader, config))
            {
                records = csv.GetRecords<CsvDataRow>().ToList();
            }

            foreach (var record in records)
            {
                var recordContent = record.Content.Trim().Split(" ");

                foreach (var word in recordContent)
                {
                    var existingPoint = dataset.DataPoints.FirstOrDefault(x => x.Word == word);

                    if (existingPoint is null)
                    {
                        dataset.DataPoints.Add(new DataPoint
                        {
                            Word = word,
                            TimesOccurredInHam = record.Classification == Classification.Ham ? 1.0 : 0.0,
                            TimesOccurredInSpam = record.Classification == Classification.Spam ? 1.0 : 0.0,
                            LastRecordContent = record.Content
                        });

                        continue;
                    }

                    if (existingPoint.LastRecordContent != record.Content)
                    {
                        var existingPointIndex = dataset.DataPoints.FindIndex(x => x.Word == word);

                        dataset.DataPoints[existingPointIndex].AddToCount(record.Classification);
                        dataset.DataPoints[existingPointIndex].LastRecordContent = record.Content;
                    }
                }

                dataset.AddToCount(record.Classification);
            }

            dataset
                .AddAlpha(1)
                .CalculatePriorProbabilities()
                .CalculateDataProbabilities();

            List<RequestDataPoint> requestList = new();

            foreach (var word in request.Content.Trim().Split(" ").ToList())
            {
                var wordInDataset = dataset.DataPoints.FirstOrDefault(x => x.Word == word);

                if (wordInDataset is null)
                {
                    continue;
                }

                var existingPoint = requestList.FirstOrDefault(x => x.Word == word);

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

                var existingPointIndex = requestList.FindIndex(x => x.Word == word);

                requestList[existingPointIndex].Count++;
            }

            var probabilityForHam = dataset.PriorHamProbability;
            var probabilityForSpam = dataset.PriorSpamProbability;

            foreach (var point in requestList)
            {
                probabilityForHam *= Math.Pow(point.HamProbability, point.Count);
                probabilityForSpam *= Math.Pow(point.SpamProbability, point.Count);
            }

            var result = probabilityForHam > probabilityForSpam ? Classification.Ham : Classification.Spam;

            return Ok(result);
        }
    }
}
