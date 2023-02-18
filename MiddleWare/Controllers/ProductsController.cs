using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiddleWare.Data;
using MiddleWare.Models.Product;
using MiddleWare.Static;

namespace MiddleWare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly MiddleWare_dbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(MiddleWare_dbContext context, IMapper mapper, ILogger<ProductsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Products
        [HttpGet]

        public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetProducts()
        {
            _logger.LogInformation($"Request to {nameof(GetProducts)}");
            try
            {
                var products = await _context.Products.Include(q => q.Customer)
                    .ProjectTo<ProductReadDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                var productsDto = _mapper.Map<IEnumerable<ProductReadDto>>(products);
                return Ok(productsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error performing GET in {nameof(GetProducts)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        
        public async Task<ActionResult<ProductDetailDto>> GetProduct(int id)
        {
            var productDto = await _context.Products
                .Include(q => q.Customer)
                .ProjectTo<ProductDetailDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(q =>q.Id == id);

            if (productDto == null)
            {
                return NotFound();
            }

            return productDto;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> PutProduct(int id, ProductUpdateDto productDto)
        {
            if (id != productDto.Id)
            {
                return BadRequest();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _mapper.Map(productDto, product);
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ProductCreateDto>> PostProduct(ProductCreateDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
