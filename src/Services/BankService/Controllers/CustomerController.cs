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

        [HttpGet("{id?}")]
        public async Task<ActionResult<Customer>> Get(string id)
        {
            // If the caller doesnt provide id we try getting it from claims
            id ??= User.Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.Name))?.Value;

            var customer = await dbContext.Customers.FindAsync(id);

            if (customer == null)
            {
                logger.LogDebug("Customer not found");
                return NotFound(id);
            }

            return Ok(customer);
        }

        [HttpPut]
        public async Task<IActionResult> Put(CustomerDto customer)
        {
            // If the caller doesnt provide id we try getting it from claims
            customer.Id ??= User.Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.Name))?.Value;

            if (customer.Id == null)
            {
                logger.LogDebug("No id provided or found for the User");
                return BadRequest("No Id provided");
            }

            var dbCustomer = await dbContext.Customers.FindAsync(customer.Id);

            if (dbCustomer == null)
            {
                logger.LogDebug($"No Customer found in db for id: {customer.Id}");
                return NotFound("Customer not found");
            }

            dbContext.Attach(dbCustomer);

            dbCustomer.RegistrationNumber = customer.RegistrationNumber;

            await dbContext.SaveChangesAsync();

            logger.LogDebug($"Customer updated with id: {customer.Id}");

            return Ok(dbCustomer);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CustomerDto customer)
        {
            // If the caller doesnt provide id we try getting it from claims
            customer.Id ??= User.FindFirstValue(JwtClaimTypes.Name);

            if (customer.Id == null)
            {
                logger.LogDebug("No id provided or found for the User");
                return BadRequest("No Id provided");
            }

            var dbCustomer = await dbContext.Customers.FindAsync(customer.Id);

            if (dbCustomer != null)
            {
                logger.LogDebug($"Customer already exists with id: {customer.Id}");
                return BadRequest("Customer already exists for this user");
            }

            dbCustomer = new Customer
            {
                Id = customer.Id,
                RegistrationNumber = customer.RegistrationNumber,
                Balance = customer.Amount
            };

            dbCustomer = (await dbContext.AddAsync(dbCustomer)).Entity;

            await dbContext.SaveChangesAsync();

            logger.LogDebug($"Customer created with id: {customer.Id}");

            return Created($"customer/{dbCustomer.Id}", dbCustomer);
        }

        [HttpPost("credits")]
        public async Task<IActionResult> PostCredits(CustomerDto customer)
        {
            // If the caller doesnt provide id we try getting it from claims
            customer.Id ??= User.Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.Name))?.Value;

            if (customer.Id == null)
            {
                return BadRequest("No Id provided");
            }

            var dbCustomer = await dbContext.Customers.FindAsync(customer.Id);

            if (dbCustomer == null)
            {
                return NotFound();
            }

            dbContext.Attach(dbCustomer);

            dbCustomer.Balance += customer.Amount;

            await dbContext.SaveChangesAsync();

            logger.LogDebug($"Credits added for Customer with id: {customer.Id}");

            return Ok();
        }
    }
}
