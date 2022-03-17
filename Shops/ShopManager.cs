using System.Collections.Generic;
using Shops.Tools;

namespace Shops
{
    public class ShopManager : IShopManager
    {
        private const int MaxSum = int.MaxValue;
        private const int MinAmountOfProduct = 0;
        private List<Shop> _shops = new List<Shop>();

        public ShopManager()
        {
        }

        public Shop AddShop(string name, string address)
        {
            var currShop = new Shop(name, address);
            _shops.Add(currShop);
            return currShop;
        }

        public void AddProductToShop(Shop shop, int price, int amount, Product product)
        {
            foreach (var item in _shops)
            {
                if (shop.GetId() == item.GetId())
                {
                    shop.AddNewProduct(price, amount, product);
                    return;
                }
            }

            throw new ShopException("This shop with name: " + shop.GetName() + "can't be found");
        }

        public Shop FindCheapBatch(Dictionary<int, Product> batch)
        {
            Shop returnShop = null;
            int minSum = MaxSum;
            foreach (Shop shop in _shops)
            {
                int currSum = 0;
                foreach (var item in batch)
                {
                    if (item.Key <= MinAmountOfProduct)
                    {
                        throw new ShopException("Amount of Product must be more than 0");
                    }

                    if (IfForCalculatingTheOrderPrice(shop, item.Value, item.Key))
                    {
                        currSum += shop.CurrentlySumParty(item.Value.GetId(), item.Key);
                    }
                    else
                    {
                        throw new ShopException("Product " + item.Value.GetName() +
                                                " not contained in this Shop or It's amount is less than you want");
                    }
                }

                if (currSum < minSum && currSum != 0)
                {
                    minSum = currSum;
                    returnShop = shop;
                }
            }

            if (returnShop == null)
            {
                throw new ShopException("There wasn't Shop where there is this party of product");
            }

            return returnShop;
        }

        public Product RegisterProduct(string name)
        {
            return new Product(name);
        }

        private bool IfForCalculatingTheOrderPrice(Shop shop, Product product, int userAmount)
        {
            return shop.IsProductWithId(product) && shop.IsNeedAmount(product, userAmount);
        }
    }
}