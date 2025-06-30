using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using api.Dtos.Stock;

namespace api.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentControllers : ControllerBase
    {
        private readonly ICommentRepository _CommentRepo;
        private readonly IStockRepository _StockRepo;

        public CommentControllers(ICommentRepository commentRepo, IStockRepository stockRepo)
        {
            _CommentRepo = commentRepo;
            _StockRepo = stockRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetComments()
        {
            var comments = await _CommentRepo.GetCommentAsync();

            var commentsDb = comments.Select(c => c.ToCommentDto());

            return Ok(commentsDb);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById([FromRoute] int id)
        {
            var comment = await _CommentRepo.GetCommentById(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId}")]

        public async Task<IActionResult> CreateCommentAsync([FromRoute] int stockId, [FromBody] CreateCommentRequestDto commentDto)
        {
            var stockExists = await _StockRepo.IsStockExistsAsync(stockId);
            if (!stockExists)
            {
                return NotFound($"Stock with ID {stockId} does not exist.");
            }
            commentDto.StockId = stockId;
            var commentModel = await _CommentRepo.CreateCommentAsync(commentDto.ToCommentFromCreateDTO());
            return CreatedAtAction(nameof(GetCommentById), new { id = commentModel.Id }, commentModel.ToCommentDto());
        }

    }
}