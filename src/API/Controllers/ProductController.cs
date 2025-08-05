using API.AutoMapper;
using API.Data;
using API.DTOs;
using API.Entities.Products;
using API.Extensions;
using API.Helpers.Contexts;
using API.Helpers.Products;
using API.RequestsHelpers;
using API.RequestsHelpers.Products;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
        public async Task<IActionResult> GetProductAll([FromQuery]ProductParams productParams)
        {
            _logger.LogInformation("GET LIST PRODUCT");
            var query = _context.DbSet<Product>()
                .OrderByCustom(productParams.OrderBy, productParams.Direction)
                .Include(x => x.Brand)
                .Include(x => x.Type)
                .Search(productParams.SearchTerm)
                .Filter(productParams.Brands, productParams.Types)
                .AsQueryable();
            
            var productDtoList =  _mapper.Map<List<ProductReturnDTO>>(await query.ToListAsync());
            var productPagination = await PagedList<ProductReturnDTO>
                                                .ToPagedList(productDtoList, productParams.PageNumber, productParams.PagesSize);
            
            Response.AddPaginationHeader(productPagination.MetaData);
            return Ok(productPagination);
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

        [HttpGet("filters")]
        public async Task<IActionResult> GetFilters()
        {
            var brands = await _context.DbSet<Product>()
                    .Select(p => p.Brand.Name).Distinct().ToListAsync();
            
            var types = await _context.DbSet<Product>()
                .Select(p => p.Type.Name).Distinct().ToListAsync();

            return Ok(new { brands, types });
        }
    }
}