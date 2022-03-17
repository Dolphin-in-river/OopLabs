using System;

namespace Banks
{
    public class DebitAccount : AbstractAccount
    {
        public DebitAccount(BankData data, double money, Client client)
            : base(data, money, client, TypeAccounts.Debit)
        {
            DebitPercent = data.DebitPercent / (MakePercent * YearDay);
        }

        public override void WithDraw(double sum)
        {
            CheckCorrectInputMoney(sum);
            CheckWithDrawSuspicion(sum);
            CheckEnoughMoney(sum);
            Money -= sum;
        }

        public override void UpdateAmount(DateTime newDate)
        {
            CheckCorrectNewDate(newDate);
            CountPayoutsPercents(newDate, DebitPercent);
        }
    }
}