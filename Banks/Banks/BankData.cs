namespace Banks
{
    public class BankData
    {
        public BankData()
        {
        }

        public BankData(double debitPercent, DepositInfo depositPercent, double creditLimit, double creditCommission, double criticalSum)
        {
            DebitPercent = debitPercent;
            InfoDeposit = depositPercent;
            CreditLimit = creditLimit;
            CreditCommission = creditCommission;
            CriticalSum = criticalSum;
        }

        public double CreditLimit
        {
            get;
            set;
        }

        public double CreditCommission
        {
            get;
            set;
        }

        public double CriticalSum
        {
            get;
            set;
        }

        public double DebitPercent
        {
            get;
            set;
        }

        public DepositInfo InfoDeposit
        {
            get;
            set;
        }

        public void SetDebitPercent(double debitPercent)
        {
            DebitPercent = debitPercent;
        }

        public void SetDepositPercent(DepositInfo depositInfo)
        {
            InfoDeposit = depositInfo;
        }

        public void SetCreditInfo(double creditLimit, double creditCommission)
        {
            CreditLimit = creditLimit;
            CreditCommission = creditCommission;
        }

        public void SetCriticalSum(double criticalSum)
        {
            CriticalSum = criticalSum;
        }
    }
}