using System.Collections.Generic;
using Shops.Tools;
using NUnit.Framework;


namespace Shops.Tests
{
    public class Tests
    {
        private IShopManager _shopManager;

        [SetUp]
        public void Setup()
        {
            _shopManager = new ShopManager();
        }
        
        [Test]
        public void AddProductToShop_BuyProductInShop()
        {
            Setup();
            Shop shop1 = _shopManager.AddShop("Lenta", "Vyazemskiy 5-7");
            Product apple = _shopManager.RegisterProduct("apple");
            Product banana = _shopManager.RegisterProduct("banana");
            _shopManager.AddProductToShop(shop1, 100, 10, apple);
            _shopManager.AddProductToShop(shop1,1000, 1, banana);
            var userOne = new Person("Ivan", 1100);
            var order = new OrderForPerson(2, apple);
            shop1.BuyProduct(userOne, order);
            if (shop1.AmountProduct(apple) == 8)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }
        
        [Test]
        public void StateAndChangePrice()
        {
            Setup();
            Shop shop1 = _shopManager.AddShop("Lenta", "Vyazemskiy 5-7");
            Product apple = _shopManager.RegisterProduct("apple");
            _shopManager.AddProductToShop(shop1,100, 10, apple);
            shop1.NewPrice(apple, 90);
            if (shop1.NewPrice(apple, 90) == 90)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }
        
        [Test]
        public void FindShopWherePriceByBatchIsCheapest()
        {
            Setup();
            Shop shop1 = _shopManager.AddShop("Lenta", "Vyazemskiy 5-7");
            Shop shop2 = _shopManager.AddShop("Magnit", "address 2");
            Product apple = _shopManager.RegisterProduct("apple");
            Product banana = _shopManager.RegisterProduct("banana");
            _shopManager.AddProductToShop(shop1,100, 10, apple);
            _shopManager.AddProductToShop(shop1,1000, 1, banana);
            
            _shopManager.AddProductToShop(shop2,1000, 10, apple);
            _shopManager.AddProductToShop(shop2,10, 100, banana);
            var list = new Dictionary<int, Product>
            {
                {11, apple},
                {5, banana}
            };
            Assert.Catch<ShopException>(() =>
            {
                _shopManager.FindCheapBatch(list);
            });
            var list2 = new Dictionary<int, Product>
            {
                {10, apple},
                {1, banana}
            };
            // in shop1 all price = 2000
            // in shop 2 all price = 10100
            if (_shopManager.FindCheapBatch(list2).GetId() == shop1.GetId())
            {
                Assert.Pass();
            }
            Assert.Fail();
        }
        
        [Test]
        public void BuyBatchInShop()
        {
            Setup();
            Shop shop1 = _shopManager.AddShop("Lenta", "Vyazemskiy 5-7");
            Product apple = _shopManager.RegisterProduct("apple");
            Product banana = _shopManager.RegisterProduct("banana");
            Product potato = _shopManager.RegisterProduct("potato");
            _shopManager.AddProductToShop(shop1,100, 100, apple);
            _shopManager.AddProductToShop(shop1,1000, 10, banana);
            _shopManager.AddProductToShop(shop1,1, 1000, potato);

            var user1 = new Person("person1", 2000);
            
            var line1 = new OrderForPerson(10, apple);   // 10 * 100 = 1000
            var line2 = new OrderForPerson(1, banana);   // 1 * 1000 = 1000
            var line3 = new OrderForPerson(11, potato);  // 11 * 1 = 11
                                                               // All price = 2011
            int remaindThatMustBe = 2000 - 2011;
            int remaindApple = 100 - 10;
            int remaindBanana = 10 - 1;
            int remaindPotato = 1000 - 11;
            var order = new List<OrderForPerson>();
            order.Add(line1);
            order.Add(line2);
            order.Add(line3);

            Assert.Catch<ShopException>(() =>
            {
                shop1.BuyBatchProduct(user1, order);
            });
            
            user1.AddMoney(1000);
            remaindThatMustBe += 1000;
            shop1.BuyBatchProduct(user1, order);
            remaindApple -= 10;
            remaindBanana -= 1;
            remaindPotato -= 11;
            if (user1.GetMoney() == remaindThatMustBe && shop1.AmountProduct(apple) == remaindApple
                                                    && shop1.AmountProduct(banana) == remaindBanana &&
                                                    shop1.AmountProduct(potato) == remaindPotato)
            {
                Assert.Pass();
            }
            Assert.Fail();
            
        }
    }
}