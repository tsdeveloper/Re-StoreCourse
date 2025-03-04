using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.AutoMapper;
using API.Data;
using API.DTOs;
using API.Entities.Products;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{   
    public class ProductController : BaseApiController
    {
        private readonly RestoreCourseDbContext _context;
        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;

        public ProductController(RestoreCourseDbContext context,
        ILogger<ProductController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductAll()
        {
            _logger.LogInformation("GET LIST PRODUCT");
            var productList = await _context.DbSet<Product>()
                                    .Include(x => x.Brand)
                                    .Include(x => x.Type)
                                    .ToListAsync();

            var returnResult = _mapper.Map<List<ProductReturnDTO>>(productList);

            return Ok(returnResult);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            _logger.LogInformation($"GET LIST PRODUCT BY ID {id}");
            var product = await _context.DbSet<Product>()
                                    .Include(x => x.Brand)
                                    .Include(x => x.Type)
                                    .FirstOrDefaultAsync(x => x.Id == id);

            if (product == null) return NotFound();

            ProductMapperDomain resultProductDto = product;

            return Ok(resultProductDto.ProductReturnDto);
        }
    }
}