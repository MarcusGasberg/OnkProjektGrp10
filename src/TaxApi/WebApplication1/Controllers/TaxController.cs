using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TaxApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaxController : ControllerBase
    {
        private readonly ILogger<TaxController> _logger;

        public TaxController(ILogger<TaxController> logger)
        {
            _logger = logger;
        }
        [HttpPost]
        [Route("tax")]
        public async Task<Tax> CalTax(Tax requestTax) {

            var tax =  new Tax(requestTax.Amount / 100, requestTax.Id);

           return tax;
        }



    }
}
