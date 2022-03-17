using System;
using BackupsExtra.CleanRestorePoints;

namespace BackupsExtra.CleanAlgorithmFactory
{
    public class OnesRequirementHybridCleanAlgorithmFactory : ICleanAlgorithmFactory
    {
        private int _amountRestorePoint;
        private DateTime _finishDate;
        public OnesRequirementHybridCleanAlgorithmFactory(int amountRestorePoint, DateTime finishDate)
        {
            _amountRestorePoint = amountRestorePoint;
            _finishDate = finishDate;
        }

        public TypeCleanPoints GetTypeClean()
        {
            return TypeCleanPoints.OnceHybrid;
        }

        public AbstractCleanAlgorithm Create()
        {
            return new OnesRequirementHybridCleanAlgorithm(_amountRestorePoint, _finishDate);
        }
    }
}