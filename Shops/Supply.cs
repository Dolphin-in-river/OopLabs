namespace Shops
{
    public class Supply
    {
        private Product _product;
        private int _price;
        private int _amount;

        public Supply(Product product, int price, int amount)
        {
            _product = product;
            _price = price;
            _amount = amount;
        }

        public int GetPrice()
        {
            return _price;
        }

        public int GetAmount()
        {
            return _amount;
        }

        public string GetName()
        {
            return _product.GetName();
        }

        public int GetId()
        {
            return _product.GetId();
        }

        public void RemoveProducts(int amount)
        {
            _amount -= amount;
        }

        public void AddProducts(int amount)
        {
            _amount += amount;
        }

        public Product GetProduct()
        {
            return _product;
        }

        public void SetNewPrice(int newPrice)
        {
            _price = newPrice;
        }
    }
}