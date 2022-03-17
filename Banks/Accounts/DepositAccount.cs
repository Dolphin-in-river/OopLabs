using System;
using Banks.Tools;

namespace Banks
{
    public class DepositAccount : AbstractAccount
    {
        public DepositAccount(BankData data, double money, DateTime finishDay, Client client)
            : base(data, money, client, TypeAccounts.Deposit)
        {
            InfoDeposit = data.InfoDeposit;
            FinishDay = finishDay;
        }

        private DateTime FinishDay
        {
            get;
        }

        public override void UpdateAmount(DateTime newDate)
        {
            if (newDate.Subtract(FinishDay).Days > 0)
            {
                throw new BanksException("Your account active until " + FinishDay);
            }

            CheckSumPositive();
            CheckCorrectNewDate(newDate);
            double oldSum = 0;
            double finallyPercent = 0;
            for (int i = 0; i < InfoDeposit.SumsInDeposits.Capacity; i++)
            {
                if (Money >= oldSum && Money <= InfoDeposit.SumsInDeposits[i])
                {
                    finallyPercent = InfoDeposit.PercentsInDeposits[i] / MakePercent;
                    break;
                }
            }

            if (finallyPercent.Equals(0))
            {
                throw new BanksException("Problem with Deposit Account");
            }

            CountPayoutsPercents(newDate, finallyPercent / YearDay);
        }

        public override void WithDraw(double sum)
        {
            CheckCorrectInputMoney(sum);
            CheckWithDrawSuspicion(sum);
            CheckEnoughMoney(sum);
            CheckDateAfterFinishDay();
            Money -= sum;
        }

        private void CheckDateAfterFinishDay()
        {
            if (FinishDay.Subtract(NowDate).Days > 0)
            {
                throw new BanksException("You can't Withdraw this Account, because your account finish day hasn't passed yet");
            }
        }
    }
}