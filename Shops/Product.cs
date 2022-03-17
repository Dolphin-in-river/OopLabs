namespace Shops
{
    public class Product
    {
        private static int _nextID = 0;
        private string _nameProduct;
        private int _id;

        public Product(string nameProduct)
        {
            _nameProduct = nameProduct;
            _id = ++_nextID;
        }

        public int GetId()
        {
            return _id;
        }

        public string GetName()
        {
            return _nameProduct;
        }
    }
}