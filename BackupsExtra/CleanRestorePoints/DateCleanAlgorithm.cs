using System;
using System.Collections.Generic;

namespace BackupsExtra.CleanRestorePoints
{
    public class DateCleanAlgorithm : AbstractCleanAlgorithm
    {
        private DateTime _finishDate;

        public DateCleanAlgorithm(DateTime finishDate)
        {
            _finishDate = finishDate;
        }

        public override List<RestorePointWithDateCreation> GetPointsAfterClean(List<RestorePointWithDateCreation> points)
        {
            var resultList = new List<RestorePointWithDateCreation>();
            foreach (RestorePointWithDateCreation item in points)
            {
                if (item.Point.CreateDataTime.Subtract(_finishDate).Seconds > 0)
                {
                    resultList.Add(item);
                }
            }

            return resultList;
        }
    }
}