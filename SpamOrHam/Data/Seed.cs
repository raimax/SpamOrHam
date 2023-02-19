using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using SpamOrHam.Enums;
using SpamOrHam.Models;
using SpamOrHam.SqlServer;
using System.Globalization;

namespace SpamOrHam.Data
{
    public class Seed
    {
        public static async Task SeedDataset(DatabaseContext context)
        {
            if (await context.Datasets.AnyAsync())
            {
                return;
            }

            try
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
                        var existingPoint = dataset.DataPoints.FirstOrDefault(x => x.Word.ToLower() == word.ToLower());

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
                            var existingPointIndex = dataset.DataPoints.FindIndex(x => x.Word.ToLower() == word.ToLower());

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

                await context.Datasets.AddAsync(dataset);

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Migration failed" + ex.Message);
                throw;
            }
        }
    }
}
