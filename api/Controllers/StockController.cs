using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using api.Data;
using api.Mappers;
using api.Dtos.Stock;
using Microsoft.EntityFrameworkCore;
using api.Interfaces;



namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        public StockController(ApplicationDbContext context, IStockRepository stockRepo)
        {
            _StockRepo = stockRepo;
            _Context = context;
        }

        private readonly ApplicationDbContext _Context;
        private readonly IStockRepository _StockRepo;

        // Add your action methods here, e.g., Get, Post, Put, Delete
        // Example:
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _StockRepo.GetAllAsync();
            var stocksDb = stocks.Select(s => s.ToStockDto());
            return Ok(stocksDb);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock =await _Context.Stock.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]

        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDTO();
            await _Context.Stock.AddAsync(stockModel);
            await _Context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());

        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            var stockModel =await _Context.Stock.FirstOrDefaultAsync(s => s.Id == id);
            if (stockModel == null)
            {
                return NotFound();
            }

            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Purchase = updateDto.Purchase;
            stockModel.LastDiv = updateDto.LastDiv;
            stockModel.Industry = updateDto.Industry;
            stockModel.MarketCap = updateDto.MarketCap;

           await _Context.SaveChangesAsync();

            return Ok(stockModel.ToStockDto());
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stockModel =await _Context.Stock.FirstOrDefaultAsync(s => s.Id == id);
            if (stockModel == null)
            {
                return NotFound();
            }
           _Context.Stock.Remove(stockModel);
           await _Context.SaveChangesAsync();
            return NoContent();
        }

    }
}