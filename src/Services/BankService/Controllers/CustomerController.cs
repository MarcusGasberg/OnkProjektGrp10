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
        private readonly ILogger<CustomerController> logger;
        private BankDbContext dbContext;

        public CustomerController(ILogger<CustomerController> logger, BankDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        [HttpGet("/{id}")]
        public async Task<ActionResult<Customer>> Get(string id)
        {

            var customer = await dbContext.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound(id);
            }

            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CustomerDto customer)
        {
            // If the caller doesnt provide id we try getting it from claims
            var id = customer.Id ?? User.Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            if (id == null)
            {
                return BadRequest("No Id provided");
            }

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
