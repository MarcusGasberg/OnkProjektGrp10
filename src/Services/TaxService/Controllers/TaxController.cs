using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TaxService
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
        public async Task<Tax> CalTax([FromBody]Tax requestTax) {

            var tax = Task.Run(()=>taxCalculation(requestTax));
            tax.Wait(); 
            
            return await tax;
        }

        public Tax taxCalculation(Tax requestTax){
            return new Tax(requestTax.Amount - requestTax.Amount/100, requestTax.Id);
        }




    }
}
