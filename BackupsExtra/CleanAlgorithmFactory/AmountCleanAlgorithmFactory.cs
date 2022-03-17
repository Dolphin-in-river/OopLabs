using BackupsExtra.CleanRestorePoints;

namespace BackupsExtra.CleanAlgorithmFactory
{
    public class AmountCleanAlgorithmFactory : ICleanAlgorithmFactory
    {
        private int _amountCleanPoint;
        public AmountCleanAlgorithmFactory(int amountCleanPoint)
        {
            _amountCleanPoint = amountCleanPoint;
        }

        public TypeCleanPoints GetTypeClean()
        {
            return TypeCleanPoints.AmountClean;
        }

        public AbstractCleanAlgorithm Create()
        {
            return new AmountCleanAlgorithm(_amountCleanPoint);
        }
    }
}