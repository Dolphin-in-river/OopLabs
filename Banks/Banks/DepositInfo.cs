using System.Collections.Generic;

namespace Banks
{
    public class DepositInfo
    {
        public DepositInfo(List<double> sumInDeposits, List<double> percentsInDeposits)
        {
            SumsInDeposits = sumInDeposits;
            PercentsInDeposits = percentsInDeposits;
        }

        public List<double> SumsInDeposits
        {
            get;
        }

        public List<double> PercentsInDeposits
        {
            get;
        }
    }
}