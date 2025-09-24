using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc; 
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
        public async Task<IActionResult> GetAllFlashcards(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "flashcards")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request to get all flashcards.");

            var flashcards = await _dbContext.Flashcards
                                             .OrderBy(f => f.CreatedDate)
                                             .ToListAsync();

            var flashcardDtos = flashcards.Select(f => new FlashcardDto
            {
                Id = f.Id.ToString(),
                Question = f.Question,
                Answer = f.Answer,
                CreatedDate = f.CreatedDate,
                LastModifiedDate = f.LastModifiedDate
            }).ToList();

            return new OkObjectResult(flashcardDtos);
        }

      
        [Function("GetFlashcardById")]
        public async Task<IActionResult> GetFlashcardById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "flashcards/{id}")] HttpRequest req,
            string id)
        {
            _logger.LogInformation($"C# HTTP trigger function processed a request to get flashcard by ID: {id}.");

            if (!Guid.TryParse(id, out var flashcardId))
            {
                return new BadRequestObjectResult("Invalid ID format.");
            }

            var flashcard = await _dbContext.Flashcards.FindAsync(flashcardId);

            if (flashcard == null)
            {
                return new NotFoundResult();
            }

            var flashcardDto = new FlashcardDto
            {
                Id = flashcard.Id.ToString(),
                Question = flashcard.Question,
                Answer = flashcard.Answer,
                CreatedDate = flashcard.CreatedDate,
                LastModifiedDate = flashcard.LastModifiedDate
            };

            return new OkObjectResult(flashcardDto);
        }

       
        [Function("GetRandomFlashcards")]
        public async Task<IActionResult> GetRandomFlashcards(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "flashcards/random")] HttpRequest req)
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
                return new NotFoundObjectResult("No flashcards found.");
            }

            var randomFlashcards = allFlashcards
                                    .OrderBy(x => _random.Next())
                                    .Take(count)
                                    .Select(f => new FlashcardDto
                                    {
                                        Id = f.Id.ToString(),
                                        Question = f.Question,
                                        Answer = f.Answer,
                                        CreatedDate = f.CreatedDate,
                                        LastModifiedDate = f.LastModifiedDate
                                    })
                                    .ToList();

            return new OkObjectResult(randomFlashcards);
        }

        [Function("CreateFlashcard")]
        public async Task<IActionResult> CreateFlashcard(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "flashcards")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request to create a new flashcard.");

            var createDto = await req.ReadFromJsonAsync<CreateFlashcardDto>();

            if (createDto == null || string.IsNullOrWhiteSpace(createDto.Question) || string.IsNullOrWhiteSpace(createDto.Answer))
            {
                return new BadRequestObjectResult("Invalid flashcard data provided.");
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

            var flashcardDto = new FlashcardDto
            {
                Id = flashcard.Id.ToString(),
                Question = flashcard.Question,
                Answer = flashcard.Answer,
                CreatedDate = flashcard.CreatedDate,
                LastModifiedDate = flashcard.LastModifiedDate
            };

            return new CreatedAtActionResult(nameof(GetFlashcardById), "FlashcardFunctions", new { id = flashcard.Id }, flashcardDto);
        }

     
        [Function("UpdateFlashcard")]
        public async Task<IActionResult> UpdateFlashcard(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "flashcards/{id}")] HttpRequest req,
            string id)
        {
            _logger.LogInformation($"C# HTTP trigger function processed a request to update flashcard with ID: {id}.");

            if (!Guid.TryParse(id, out var flashcardId))
            {
                return new BadRequestObjectResult("Invalid ID format.");
            }

            var updateDto = await req.ReadFromJsonAsync<UpdateFlashcardDto>();

            if (updateDto == null)
            {
                return new BadRequestObjectResult("Invalid update data provided.");
            }

            var flashcard = await _dbContext.Flashcards.FindAsync(flashcardId);

            if (flashcard == null)
            {
                return new NotFoundResult();
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

            return new NoContentResult();
        }

     
        [Function("DeleteFlashcard")]
        public async Task<IActionResult> DeleteFlashcard(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "flashcards/{id}")] HttpRequest req,
            string id)
        {
            _logger.LogInformation($"C# HTTP trigger function processed a request to delete flashcard with ID: {id}.");

            if (!Guid.TryParse(id, out var flashcardId))
            {
                return new BadRequestObjectResult("Invalid ID format.");
            }

            var flashcard = await _dbContext.Flashcards.FindAsync(flashcardId);

            if (flashcard == null)
            {
                return new NotFoundResult();
            }

            _dbContext.Flashcards.Remove(flashcard);
            await _dbContext.SaveChangesAsync();

            return new NoContentResult();
        }
    }
}
