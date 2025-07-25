using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Dtos.Stock;
using api.Models;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetCommentAsync();

        Task<Comment?> GetCommentById(int id);

        Task<Comment> CreateCommentAsync(Comment commentModel);

        Task<Comment?> UpdateCommentAsync(int id, UpdateCommentDto commentDto);

        Task<Comment?> DeleteCommentAsync(int id);
    }
}