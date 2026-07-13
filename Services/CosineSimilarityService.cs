using Microsoft.EntityFrameworkCore;
using TravelAI.Data;
using TravelAI.Models.DTOs;
using TravelAI.Models.Entities;

namespace TravelAI.Services
{
    public class CosineSimilarityService
    {
        private readonly AppDbContext _context;

        public CosineSimilarityService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<RecommendationResult>> RecommendDestinations(RecommendationRequest request)
        {
            
            // Load destination features with destination info
            var features = await _context.DestinationFeatures
                .Include(x => x.Destination)
                .ToListAsync();

            var userVector = BuildUserVector(request);

            List<RecommendationResult> recommendations = new();

            foreach (var feature in features)
            {
                var destinationVector = BuildDestinationVector(feature);

                double similarity = CalculateCosineSimilarity(userVector, destinationVector);

                recommendations.Add(new RecommendationResult
                {
                    DestinationId = feature.DestinationId,
                    DestinationName = feature.Destination?.Name ?? "",
                    Similarity = Math.Round(similarity, 4)
                });
            }

            // Sort + return top results
            return recommendations
                .OrderByDescending(x => x.Similarity)
                .Take(10)
                .ToList();
        }

        // ---------------- VECTOR BUILDERS ----------------

        private double[] BuildUserVector(RecommendationRequest request)
        {
            return new double[]
            {
                Clamp(request.Adventure),
                Clamp(request.Nature),
                Clamp(request.Culture),
          
                Clamp(request.Wildlife),
                Clamp(request.Trekking),
               
         
                Clamp(request.Religious),
                Clamp(request.NightLife)
            };
        }

        private double[] BuildDestinationVector(DestinationFeature feature)
        {
            return new double[]
            {
                feature.Adventure,
                   feature.Nature,
                   feature.Wildlife,
                   feature.Religious,
                   feature.Culture,
                   feature.Luxury,
                   feature.Trekking
             
            };
        }

        // ---------------- COSINE SIMILARITY ----------------

        private double CalculateCosineSimilarity(double[] vectorA, double[] vectorB)
        {
            double dotProduct = 0;
            double magnitudeA = 0;
            double magnitudeB = 0;

            for (int i = 0; i < vectorA.Length; i++)
            {
                dotProduct += vectorA[i] * vectorB[i];
                magnitudeA += Math.Pow(vectorA[i], 2);
                magnitudeB += Math.Pow(vectorB[i], 2);
            }

            magnitudeA = Math.Sqrt(magnitudeA);
            magnitudeB = Math.Sqrt(magnitudeB);

            if (magnitudeA == 0 || magnitudeB == 0)
                return 0;

            return dotProduct / (magnitudeA * magnitudeB);
        }

        // ---------------- SAFETY ----------------

        private double Clamp(double value)
        {
            return Math.Max(0, Math.Min(10, value));
        }
    }
}