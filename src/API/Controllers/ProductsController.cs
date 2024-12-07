using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{   
    public class ProductsController : BaseApiController
    {
        private readonly RestoreCourseDbContext _context;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(RestoreCourseDbContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductAll()
        {
            _logger.LogInformation("GET LIST PRODUCT");
            var productList = await _context.DbSet<Product>().ToListAsync();

            return Ok(productList);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            _logger.LogInformation($"GET LIST PRODUCT BY ID {id}");
            var product = await _context.DbSet<Product>().FirstOrDefaultAsync(x => x.Id == id);

            return Ok(product);
        }
    }
}