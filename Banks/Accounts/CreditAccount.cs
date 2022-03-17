using System;
using Banks.Tools;

namespace Banks
{
    public class CreditAccount : AbstractAccount
    {
        private bool _usedLimit = false;
        public CreditAccount(BankData data, double money, Client client)
            : base(data, money, client, TypeAccounts.Credit)
        {
            CreditLimit = data.CreditLimit;
            CreditCommission = data.CreditCommission / YearDay;
        }

        public override void UpdateAmount(DateTime newDate)
        {
            CheckCorrectNewDate(newDate);
            if (_usedLimit)
            {
                CheckSumPositive();
                int changeDays = newDate.Subtract(NowDate).Days;
                NowDate = newDate;
                int remainderLastMonth = LastChangedDays % MonthDay;
                int amountMonthSinceLastTime = (changeDays + remainderLastMonth) / MonthDay;
                Money -= CreditCommission * MonthDay * amountMonthSinceLastTime;
                LastChangedDays = changeDays;
            }
        }

        public override void WithDraw(double sum)
        {
            CheckCorrectInputMoney(sum);
            CheckWithDrawSuspicion(sum);
            CheckEnoughMoney(sum);
            Money -= sum;
            if (Money < 0)
            {
                Money += CreditLimit;
                _usedLimit = true;
            }
        }

        protected override void CheckEnoughMoney(double sum)
        {
            if ((sum - Money > CreditLimit) || (sum > Money && _usedLimit))
            {
                throw new BanksException("Not enough money");
            }
        }
    }
}