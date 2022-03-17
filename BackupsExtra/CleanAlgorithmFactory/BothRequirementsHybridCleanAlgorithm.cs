using System;
using BackupsExtra.CleanRestorePoints;

namespace BackupsExtra.CleanAlgorithmFactory
{
    public class BothRequirementsHybridCleanAlgorithm : ICleanAlgorithmFactory
    {
        private DateTime _finishDate;
        private int _amountRestorePoint;
        public BothRequirementsHybridCleanAlgorithm(int amountRestorePoint, DateTime finishDate)
        {
            _finishDate = finishDate;
            _amountRestorePoint = amountRestorePoint;
        }

        public TypeCleanPoints GetTypeClean()
        {
            return TypeCleanPoints.BothHybrid;
        }

        public AbstractCleanAlgorithm Create()
        {
            return new BothRequirementsHybridClean(_amountRestorePoint, _finishDate);
        }
    }
}