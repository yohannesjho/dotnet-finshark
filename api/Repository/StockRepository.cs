using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Interfaces;
using api.Models;
using api.Dtos.Stock;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _Context;

        public StockRepository(ApplicationDbContext context) // ✅ Constructor must be public
        {
            _Context = context;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _Context.Stock.AddAsync(stockModel);
            await _Context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _Context.Stock.FirstOrDefaultAsync(s => s.Id == id);

            if (stockModel == null)
            {
                return null;
            }
            _Context.Stock.Remove(stockModel);
            await _Context.SaveChangesAsync();

            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            return await _Context.Stock.Include(c => c.Comments).ToListAsync(); // ✅ Must return the Task
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            var  stockModel = await _Context.Stock.Include(c => c.Comments).FirstOrDefaultAsync(s => s.Id == id);

            if (stockModel == null)
            {
                return null;
            }
            return stockModel;
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateDto)
        {

            var stockModel = await _Context.Stock.FirstOrDefaultAsync(s => s.Id == id);
            if (stockModel == null)
            {
                return null;
            }
            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Purchase = updateDto.Purchase;
            stockModel.LastDiv = updateDto.LastDiv;
            stockModel.Industry = updateDto.Industry;
            stockModel.MarketCap = updateDto.MarketCap;

            return stockModel;
        }
    }
}