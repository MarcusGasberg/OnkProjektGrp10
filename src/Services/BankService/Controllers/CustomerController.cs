using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using IdentityModel;
using BankService.Controllers.DataTransfer;
using BankService.Data;
using BankService.Models;

namespace BankService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private BankDbContext dbContext;

        public CustomerController(ILogger<CustomerController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/{id}")]
        public async Task<ActionResult<Customer>> Get(string id)
        {
            using (dbContext = new BankDbContext())
            {
                var customer = await dbContext.Customers.FindAsync(id);

                if (customer == null)
                {
                    return NotFound(id);
                }

                return Ok(customer);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(CustomerDto customer)
        {
            using (dbContext = new BankDbContext())
            {
                // If id not provided we try getting it from the user
                var id = customer.Id ?? User.Claims.FirstOrDefault(c => c.GetType().Equals(JwtClaimTypes.Subject))?.Value;

                var dbCustomer = new Customer
                {
                    Id = id,
                    RegistrationNumber = customer.RegistrationNumber
                };

                dbCustomer = (await dbContext.AddAsync(dbCustomer)).Entity;

                await dbContext.SaveChangesAsync();

                return Created($"customer/{dbCustomer.Id}", dbCustomer);
            }
        }
    }
}
