using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentControllers : ControllerBase
    {
        private readonly ICommentRepository _CommentRepo;

        public CommentControllers(ICommentRepository commentRepo)
        {
            _CommentRepo = commentRepo;
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

    }
}