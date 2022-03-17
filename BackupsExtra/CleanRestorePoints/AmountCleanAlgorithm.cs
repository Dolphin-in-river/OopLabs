using System.Collections.Generic;
namespace BackupsExtra.CleanRestorePoints
{
    public class AmountCleanAlgorithm : AbstractCleanAlgorithm
    {
        private int _amountPoint;

        public AmountCleanAlgorithm(int amountPoint)
        {
            _amountPoint = amountPoint;
        }

        public override List<RestorePointWithDateCreation> GetPointsAfterClean(List<RestorePointWithDateCreation> points)
        {
            var pointsAfterClean = new List<RestorePointWithDateCreation>();
            for (int i = points.Count - _amountPoint; i < points.Count; i++)
            {
                pointsAfterClean.Add(points[i]);
            }

            return pointsAfterClean;
        }
    }
}