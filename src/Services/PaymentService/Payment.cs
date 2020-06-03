namespace PaymentApi
{
    public class Payment
    {
        private int _amount;


        public Payment(int amount)
        {
            _amount = amount;
        }


        public int GetAmount()
        {
            return _amount;
        }

        public void SetAmount(int amount)
        {
            _amount = amount;
        }
    }
}