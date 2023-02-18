using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiddleWare.Data;
using MiddleWare.Models.Customer;
using MiddleWare.Static;

namespace MiddleWare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly MiddleWare_dbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(MiddleWare_dbContext context, IMapper mapper, ILogger<CustomersController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerReadDto>>> GetCustomers()
        {
            _logger.LogInformation($"Request to {nameof(GetCustomers)}");
            try
            {
                var customers = await _context.Customers.ToListAsync();
                var customersDto = _mapper.Map<IEnumerable<CustomerReadDto>>(customers);
                return Ok(customersDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error performing GET in {nameof(GetCustomers)}");
                return StatusCode(500, Messages.Error500Message);
            }

        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerReadDto>> GetCustomer(int id)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null)
                {
                    _logger.LogWarning($"Record not found: {nameof(GetCustomer)} - ID: {id}");
                    return NotFound();
                }
                var customerDto = _mapper.Map<CustomerReadDto>(customer);
                return Ok(customerDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error performing GET in {nameof(GetCustomer)}");
                return StatusCode(500, Messages.Error500Message);
            }

        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, CustomerUpdateDto customerDto)
        {
            if (id != customerDto.Id)
            {
                return BadRequest();
            }
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }
            _mapper.Map(customerDto, customer);
            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CustomerExists(id))
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

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CustomerCreateDto>> PostCustomer(CustomerCreateDto customerDto)
        {
            try
            {
                var customer = _mapper.Map<Customer>(customerDto);
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error performing POST in {nameof(PostCustomer)}");
                return StatusCode(500, Messages.Error500Message);
            }
            
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }

                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Error performing DELETE in {nameof(DeleteCustomer)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        private async Task<bool> CustomerExists(int id)
        {
            return await _context.Customers.AnyAsync(e => e.Id == id);
        }
    }
}
