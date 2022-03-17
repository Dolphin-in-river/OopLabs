using System;
using System.Collections.Generic;

namespace BackupsExtra.CleanRestorePoints
{
    public class BothRequirementsHybridClean : AbstractCleanAlgorithm
    {
        private int _amountRestorePoint;
        private DateTime _finishDate;

        public BothRequirementsHybridClean(int amountRestorePoint, DateTime finishDate)
        {
            _amountRestorePoint = amountRestorePoint;
            _finishDate = finishDate;
        }

        public override List<RestorePointWithDateCreation> GetPointsAfterClean(List<RestorePointWithDateCreation> points)
        {
            var firstResultList = new List<RestorePointWithDateCreation>();
            var secondResultList = new List<RestorePointWithDateCreation>();
            var resultList = new List<RestorePointWithDateCreation>();
            for (int i = _amountRestorePoint; i < points.Count; i++)
            {
                firstResultList.Add(points[i]);
            }

            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Point.CreateDataTime.Subtract(_finishDate).Days > 0)
                {
                    secondResultList.Add(points[i]);
                }
            }

            for (int i = 0; i < points.Count; i++)
            {
                if (firstResultList.Contains(points[i]) || secondResultList.Contains(points[i]))
                {
                    resultList.Add(points[i]);
                }
            }

            return resultList;
        }
    }
}