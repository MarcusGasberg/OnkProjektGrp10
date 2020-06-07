using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxService
{
    public class Tax
    {
        public int Amount { set; get; }
        public string Id { set; get; }

        public Tax(int amount, string id)
        {
            Amount = amount;
            Id = id;
        }

        public Tax()
        {
            
        }

    }
}
