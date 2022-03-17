namespace Banks
{
    public interface IBankBuilder
    {
        IBankBuilder BuildName(string bankName);
        IBankBuilder BuildDebitPercent(double debitPercent);
        IBankBuilder BuildDepositInfo(DepositInfo depositInfo);
        IBankBuilder BuildCreditLimit(double creditLimit);
        IBankBuilder BuildCreditCommission(double creditCommission);
        IBankBuilder BuildCriticalSum(double criticalSum);
        Bank GetResult();
    }
}