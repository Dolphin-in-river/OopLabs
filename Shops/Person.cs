using System.Collections.Generic;

namespace Shops
{
    public class Person
    {
        private string _name;
        private int _moneyForProducts;
        private List<OrderForPerson> _purchases = new List<OrderForPerson>();
        public Person(string name, int moneyForProducts)
        {
            _name = name;
            _moneyForProducts = moneyForProducts;
        }

        public Person(string name, int moneyForProducts, List<OrderForPerson> purchases)
        {
            _name = name;
            _moneyForProducts = moneyForProducts;
            _purchases = purchases;
        }

        public void AddMoney(int newMoney)
        {
            _moneyForProducts += newMoney;
        }

        public int GetMoney()
        {
            return _moneyForProducts;
        }

        public int MoneyAfterBuy(int buffMoney)
        {
            _moneyForProducts -= buffMoney;
            return _moneyForProducts;
        }

        public void AddOrder(OrderForPerson order)
        {
            _purchases.Add(order);
        }
    }
}