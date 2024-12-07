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
    public class ProductsApiController : BaseApiController
    {
        private readonly RestoreCourseDbContext _context;

        public ProductsApiController(RestoreCourseDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductAll()
        {
            var productList = await _context.DbSet<Product>().ToListAsync();

            return Ok(productList);
        }
    }
}