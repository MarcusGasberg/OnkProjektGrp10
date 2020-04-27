using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxApi
{
    public class Tax
    {
        private int _amount;

        public Tax(int amount)
        {
            _amount = amount;
        }

        public int getAmount()
        {
            return _amount;
        }

        public void setAmount(int amount)
        {
            _amount = amount;
        }

    }
}
