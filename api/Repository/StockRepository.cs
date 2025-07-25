using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Interfaces;
using api.Models;
using api.Dtos.Stock;
using api.Helper;

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

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stocks = _Context.Stock.Include(c => c.Comments).AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {

                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                }
            }

            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            }

            var SkipNumber = (query.PageNumber - 1) * query.PageSize;


            return await stocks.Skip(SkipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            var stockModel = await _Context.Stock.Include(c => c.Comments).FirstOrDefaultAsync(s => s.Id == id);

            if (stockModel == null)
            {
                return null;
            }
            return stockModel;
        }

        public async Task<bool> IsStockExistsAsync(int id)
        {
            return await _Context.Stock.AnyAsync(s => s.Id == id);

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