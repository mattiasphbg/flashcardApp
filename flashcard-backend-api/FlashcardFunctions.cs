using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using flashcard_backend_api.Data;
using flashcard.Shared.DTOs; 
using flashcard.Shared.Models; 
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNetCore.JsonPatch;

namespace flashcard_backend_api
{
    public class FlashcardFunctions
    {
        private readonly ILogger<FlashcardFunctions> _logger;
        private readonly AppDbContext _dbContext;
        private readonly Random _random;

        public FlashcardFunctions(ILogger<FlashcardFunctions> logger, AppDbContext dbContext, Random random)
        {
            _logger = logger;
            _dbContext = dbContext;
            _random = random;
        }

       
        [Function("GetAllFlashcards")]
        public async Task<HttpResponseData> GetAllFlashcards(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "flashcards")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request to get all flashcards.");

            var flashcards = await _dbContext.Flashcards
                                             .OrderBy(f => f.CreatedDate)
                                             .ToListAsync();

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(flashcards);
            return response;
        }

      
        [Function("GetFlashcardById")]
        public async Task<HttpResponseData> GetFlashcardById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "flashcards/{id}")] HttpRequestData req,
            string id
        )
        {
            _logger.LogInformation($"C# HTTP trigger function processed a request to get flashcard by ID: {id}.");

            if (!Guid.TryParse(id, out var flashcardId))
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            var flashcard = await _dbContext.Flashcards.FindAsync(flashcardId);

            if (flashcard == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(flashcard);
            return response;
        }

       
        [Function("GetRandomFlashcards")]
        public async Task<HttpResponseData> GetRandomFlashcards(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "flashcards/random")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request to get random flashcards.");

            int count = 1;

            string countString = req.Query["count"];
            if (!string.IsNullOrEmpty(countString) && int.TryParse(countString, out int parsedCount))
            {
                count = Math.Max(1, parsedCount);
            }

            var allFlashcards = await _dbContext.Flashcards.ToListAsync();

            if (!allFlashcards.Any())
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            var randomFlashcards = allFlashcards
                                    .OrderBy(x => _random.Next())
                                    .Take(count)
                                    .ToList();

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(randomFlashcards);
            return response;
        }

        [Function("CreateFlashcard")]
        public async Task<HttpResponseData> CreateFlashcard(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "flashcards")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request to create a new flashcard.");

            var createDto = await req.ReadFromJsonAsync<CreateFlashcardDto>();

            if (createDto == null || string.IsNullOrWhiteSpace(createDto.Question) || string.IsNullOrWhiteSpace(createDto.Answer))
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            var flashcard = new Flashcard
            {
                Id = Guid.NewGuid(),
                Question = createDto.Question,
                Answer = createDto.Answer,
                CreatedDate = DateTime.UtcNow
            };

            _dbContext.Flashcards.Add(flashcard);
            await _dbContext.SaveChangesAsync();

            var response = req.CreateResponse(HttpStatusCode.Created);
            response.Headers.Add("Location", $"/flashcards/{flashcard.Id}");
            await response.WriteAsJsonAsync(flashcard);
            return response;
        }

     
        [Function("UpdateFlashcard")]
        public async Task<HttpResponseData> UpdateFlashcard(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "flashcards/{id}")] HttpRequestData req,
            string id)
        {
            _logger.LogInformation($"C# HTTP trigger function processed a request to update flashcard with ID: {id}.");

            if (!Guid.TryParse(id, out var flashcardId))
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            var updateDto = await req.ReadFromJsonAsync<UpdateFlashcardDto>();

            if (updateDto == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            var flashcard = await _dbContext.Flashcards.FindAsync(flashcardId);

            if (flashcard == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Question))
            {
                flashcard.Question = updateDto.Question;
            }
            if (!string.IsNullOrWhiteSpace(updateDto.Answer))
            {
                flashcard.Answer = updateDto.Answer;
            }
            flashcard.LastModifiedDate = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return req.CreateResponse(HttpStatusCode.NoContent);
        }

     
        [Function("DeleteFlashcard")]
        public async Task<HttpResponseData> DeleteFlashcard(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "flashcards/{id}")] HttpRequestData req,
            string id)
        {
            _logger.LogInformation($"C# HTTP trigger function processed a request to delete flashcard with ID: {id}.");

            if (!Guid.TryParse(id, out var flashcardId))
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            var flashcard = await _dbContext.Flashcards.FindAsync(flashcardId);

            if (flashcard == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            _dbContext.Flashcards.Remove(flashcard);
            await _dbContext.SaveChangesAsync();

            return req.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}
