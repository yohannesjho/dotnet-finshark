using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Comment;
using api.Dtos.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _Context;

        public CommentRepository(ApplicationDbContext context)
        {
            _Context = context;
        }

        public async Task<Comment> CreateCommentAsync(Comment commentModel)
        {

            await _Context.Comment.AddAsync(commentModel);
            await _Context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<Comment?> DeleteCommentAsync(int id)
        {
            var commentModel = await _Context.Comment.FirstOrDefaultAsync(c => c.Id == id);
            if (commentModel == null)
            {
                return null;
            }

            _Context.Comment.Remove(commentModel);
            await _Context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<List<Comment>> GetCommentAsync()
        {
            return await _Context.Comment.ToListAsync();
            
        }

        public async Task<Comment?> GetCommentById(int id)
        {
            return await _Context.Comment.FirstOrDefaultAsync(c => c.Id == id);
                       
        }

        public async Task<Comment?> UpdateCommentAsync(int id, UpdateCommentDto commentDto)
        {
            var commentModel = await _Context.Comment.FirstOrDefaultAsync(c => c.Id == id);
            if (commentModel == null)
            {
                return null;
            }

            commentModel.Title = commentDto.Title;
            commentModel.Content = commentDto.Content;
            _Context.SaveChanges();
            return commentModel;
        }
    }
}