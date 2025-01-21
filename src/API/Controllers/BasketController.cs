using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities.Baskets;
using API.Entities.Products;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{   
    public class BasketController : BaseApiController
    {
        private readonly RestoreCourseDbContext _context;
        private readonly ILogger<BasketController> _logger;
        private readonly IMapper _mapper;

        public BasketController(RestoreCourseDbContext context,
        ILogger<BasketController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            _logger.LogInformation("GET BASKET");
            var basket = await _context.DbSet<Basket>()
                .Include(x => x.BasketItems)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.BuyerId == Request.Cookies["buyerId"]);
            
            if (basket == null) return NotFound();

            var returnResult = _mapper.Map<List<BaskReturnDTO>>(basket);

            return Ok(returnResult);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddItemToBasket(int productId, int quantity)
        {
            return CreatedAtAction(nameof(GetBasket),new BaskReturnDTO());
        }

        [HttpGet]
        public async Task<IActionResult> GetBasketAll()
        {
            _logger.LogInformation("GET LIST BASKET ALL");
            var productList = await _context.DbSet<Basket>()
                                    .Include(x => x.BasketItems)
                                    .ToListAsync();

            var returnResult = _mapper.Map<List<BaskReturnDTO>>(productList);

            return Ok(returnResult);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBasketById(int id)
        {
            _logger.LogInformation($"GET BASKET BY ID {id}");
            var product = await _context.DbSet<Basket>()
                                    .Include(x => x.BasketItems)
                                    .FirstOrDefaultAsync(x => x.Id == id);

            if (product == null) return NotFound();

            return Ok(product);
        }
        
        [HttpDelete]
        public async Task<IActionResult> RemoveBasketItem(int productId, int quantity)
        {
            return CreatedAtAction(nameof(GetBasket),new BaskReturnDTO());
        }
    }
}