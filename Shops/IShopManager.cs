using System.Collections.Generic;

namespace Shops
{
    public interface IShopManager
    {
        Shop AddShop(string name, string address);
        Shop FindCheapBatch(Dictionary<int, Product> batch);
        Product RegisterProduct(string name);
        void AddProductToShop(Shop shop, int price, int amount, Product product);
    }
}