namespace Shops
{
    public class OrderForPerson
    {
        private int _amount;
        private Product _product;

        public OrderForPerson(int amount, Product product)
        {
            _amount = amount;
            _product = product;
        }

        public int GetId()
        {
            return _product.GetId();
        }

        public string GetName()
        {
            return _product.GetName();
        }

        public int GetAmount()
        {
            return _amount;
        }
    }
}