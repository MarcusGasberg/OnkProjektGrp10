using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxApi
{
    public class Tax
    {
        private int _amount;
        private string _id;

        public Tax(int amount, string id)
        {
            _amount = amount;
            _id = id;
        }

        public int getAmount()
        {
            return _amount;
        }

        public void setAmount(int amount)
        {
            _amount = amount;
        }

        public string getId()
        {
            return _id;
        }

        public void setId(string id)
        {
            _id = id;
            
        }

    }
}
