using System;
using BackupsExtra.CleanRestorePoints;

namespace BackupsExtra.CleanAlgorithmFactory
{
    public class DateCleanAlgorithmFactory : ICleanAlgorithmFactory
    {
        private DateTime _finishDate;
        public DateCleanAlgorithmFactory(DateTime finishDate)
        {
            _finishDate = finishDate;
        }

        public TypeCleanPoints GetTypeClean()
        {
            return TypeCleanPoints.DateClean;
        }

        public AbstractCleanAlgorithm Create()
        {
            return new DateCleanAlgorithm(_finishDate);
        }
    }
}