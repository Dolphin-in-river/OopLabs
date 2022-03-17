using System;
using Banks.Tools;

namespace Banks
{
    public abstract class AbstractAccount
    {
        protected const int MakePercent = 100;
        protected const double YearDay = 365.0;
        protected const int MonthDay = 30;
        private static int _nextId = 1;
        private Client _client;
        private int _id;

        protected AbstractAccount(BankData data, double money, Client client, TypeAccounts type)
        {
            Money = money;
            CriticalSum = data.CriticalSum;
            NowDate = DateTime.Today;
            _id = _nextId++;
            _client = client;
            LastChangedDays = 0;
            TypeAccount = type;
        }

        public TypeAccounts TypeAccount
        {
            get;
        }

        public double CriticalSum
        {
            get;
            protected set;
        }

        public double Money
        {
            get;
            protected set;
        }

        protected int LastChangedDays
        {
            get;
            set;
        }

        protected DateTime NowDate
        {
            get;
            set;
        }

        protected double CreditLimit
        {
            get;
            set;
        }

        protected double CreditCommission
        {
            get;
            set;
        }

        protected DepositInfo InfoDeposit
        {
            get;
            set;
        }

        protected double DebitPercent
        {
            get;
            set;
        }

        public int GetId()
        {
            return _id;
        }

        public abstract void UpdateAmount(DateTime newDate);

        public void AddMoney(double sum)
        {
            CheckCorrectInputMoney(sum);
            Money += sum;
        }

        public Client GetClient()
        {
            return _client;
        }

        public abstract void WithDraw(double sum);

        public void UpdateInformation(BankData data)
        {
            DebitPercent = data.DebitPercent / (MakePercent * YearDay);
            CriticalSum = data.CriticalSum;
            InfoDeposit = data.InfoDeposit;
            CreditLimit = data.CreditLimit;
            CreditCommission = data.CreditCommission / YearDay;
        }

        public string GetInformation()
        {
            return GetId().ToString();
        }

        protected void CheckWithDrawSuspicion(double sum)
        {
            if (sum > CriticalSum && !GetSafely())
            {
                throw new BanksException("You are Suspicion person! And you can't get money from Account");
            }
        }

        protected void CheckCorrectInputMoney(double sum)
        {
            if (sum < 0)
            {
                throw new BanksException("This sum is incorrect");
            }
        }

        protected void CheckSumPositive()
        {
            if (Money < 0)
            {
                throw new BanksException("Your balance is less than 0");
            }
        }

        protected virtual void CheckEnoughMoney(double sum)
        {
            if (Money < sum)
            {
                throw new BanksException("Not enough money");
            }
        }

        protected void CheckCorrectNewDate(DateTime newDate)
        {
            if (newDate.Subtract(NowDate).Days < 0)
            {
                throw new BanksException("Your Date is incorrect. " + newDate + "must be later than " + NowDate);
            }
        }

        protected void CountPayoutsPercents(DateTime newDate, double percent)
        {
            CheckSumPositive();
            int changeDays = newDate.Subtract(NowDate).Days;
            NowDate = newDate;
            int remainderLastMonth = LastChangedDays % MonthDay;
            int amountMonthSinceLastTime = (changeDays + remainderLastMonth) / MonthDay;
            Money += Money * percent * MonthDay * amountMonthSinceLastTime;
            LastChangedDays = changeDays;
        }

        private bool GetSafely()
        {
            return _client.CheckNotRequireFields();
        }
    }
}