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
        public async Task<Tax> CalTax(int money, string id) {

            var tax =  new Tax(money / 100, id);

           return tax;
        }



    }
}
