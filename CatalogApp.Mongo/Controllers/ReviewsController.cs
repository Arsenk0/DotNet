using CatalogApp.Mongo.Entities;
using CatalogApp.Mongo.Features.Reviews.Queries.GetReviews;
using CatalogApp.Mongo.Features.Reviews.Queries.GetReviewById;
using CatalogApp.Mongo.Features.Reviews.Queries.GetProductStats;
using CatalogApp.Mongo.Features.Reviews.Commands.CreateReview;
using CatalogApp.Mongo.Features.Reviews.Commands.AddComment;
// Додаємо нові usings для Update та Delete
using CatalogApp.Mongo.Features.Reviews.Commands.UpdateReview;
using CatalogApp.Mongo.Features.Reviews.Commands.DeleteReview;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CatalogApp.Mongo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReviewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 1. Отримати всі відгуки
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            var result = await _mediator.Send(new GetReviewsQuery());
            return Ok(result);
        }

        // 2. Отримати один відгук по ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetById(string id)
        {
            var result = await _mediator.Send(new GetReviewByIdQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        // 3. Статистика по товару (Aggregation)
        [HttpGet("stats/{productId:int}")]
        public async Task<ActionResult<ProductStatsDto>> GetStats(int productId)
        {
            var result = await _mediator.Send(new GetProductStatsQuery(productId));
            return Ok(result);
        }

        // 4. Створити відгук
        [HttpPost]
        public async Task<ActionResult<Review>> CreateReview([FromBody] CreateReviewCommand command)
        {
            // Тут автоматично спрацює FluentValidation, якщо ви його зареєстрували
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // 5. Оновити відгук (PUT) - НОВЕ
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(string id, [FromBody] UpdateReviewCommand command)
        {
            if (id != command.Id && string.IsNullOrEmpty(command.Id))
            {
                command.Id = id;
            }

            var success = await _mediator.Send(command);
            
            if (!success) return NotFound();
            
            return NoContent();
        }

        // 6. Видалити відгук (DELETE) - НОВЕ
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(string id)
        {
            var success = await _mediator.Send(new DeleteReviewCommand(id));
            
            if (!success) return NotFound();
            
            return NoContent();
        }
        
        // 7. Додати коментар (вкладений документ)
        [HttpPost("{id}/comments")]
        public async Task<ActionResult> AddComment(string id, [FromBody] AddCommentCommand command)
        {
            command.ReviewId = id;
            var success = await _mediator.Send(command);

            if (!success)
            {
                return NotFound($"Review with id {id} not found");
            }

            return NoContent();
        }
    }
}