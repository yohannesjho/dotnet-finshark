using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Interfaces;
using api.Models;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _Context;

        public StockRepository(ApplicationDbContext context) // ✅ Constructor must be public
        {
            _Context = context;
        }

        public Task<List<Stock>> GetAllAsync()
        {
            return _Context.Stock.ToListAsync(); // ✅ Must return the Task
        }
    }
}