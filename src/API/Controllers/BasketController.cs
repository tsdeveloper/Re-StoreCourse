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
        public async Task<IActionResult> GetBasketAll()
        {
            _logger.LogInformation("GET LIST BASKET ALL");
            var basketList = await _context.DbSet<Basket>()
                .Include(x => x.BasketItems)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.BuyerId == Request.Cookies["buyerId"]);

            var returnResult = _mapper.Map<List<BasketReturnDTO>>(basketList);

            return Ok(returnResult);
        }
       
        
        [HttpPost]
        public async Task<IActionResult> AddItemToBasket(int productId, int quantity)
        {
            _logger.LogInformation("ADD BASKET");
            var basket = await RetrieveBasket();

            if (basket == null) basket = CreateBasket();

            var product = await _context.DbSet<Product>().FindAsync(productId);
            
            if (basket == null) return NotFound();
            
            basket.AddItem(product, quantity);
            
            var resultBasketSave = await _context.SaveChangesAsync() > 0;   
            
            if (!resultBasketSave) return BadRequest( new ProblemDetails {Title = "Problem saving item to Basket"});

            var basketDto = _mapper.Map<BasketReturnDTO>(basket);
            return CreatedAtAction(nameof(GetBasketById), new { id = basketDto.Id }, basketDto);
        }
       
        private async Task<Basket?> RetrieveBasket()
        {
            var basket = await _context.DbSet<Basket>()
                .Include(x => x.BasketItems)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.BuyerId == Request.Cookies["buyerId"]);
            return basket;
        }

        private Basket CreateBasket()
        {
            var buyerId = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions
            {
                IsEssential = true,
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            };
            
            Response.Cookies.Append("buyerId", buyerId, cookieOptions);
            var basket = new Basket { BuyerId = buyerId };
            _context.DbSet<Basket>().Add(basket);
            
            return basket;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBasketById(int id)
        {
            _logger.LogInformation($"GET BASKET BY ID {id}");
            var basket = await _context.DbSet<Basket>()
                .Include(x => x.BasketItems)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (basket == null) return NotFound();

            return Ok(basket);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveBasketItem(int productId, int quantity)
        {
            return NoContent();
        }
    }
}