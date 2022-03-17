using System.Collections.Generic;
using System.IO;
using Backups;
using BackupsExtra.Tools;

namespace BackupsExtra.CleanRestorePoints
{
    public abstract class AbstractCleanAlgorithm
    {
        public abstract List<RestorePointWithDateCreation> GetPointsAfterClean(List<RestorePointWithDateCreation> points);

        public void Merge(List<RestorePointWithDateCreation> points, bool localKeep)
        {
            RestorePointWithDateCreation firstSplitPoint = null;
            int countSplitPoint = 0;
            foreach (RestorePointWithDateCreation item in GetPointsAfterClean(points))
            {
                if (item.Point.GetType() == typeof(SplitRestorePoint))
                {
                    firstSplitPoint = item;
                    break;
                }
            }

            for (int i = 0; i < CountPointsWhichDeleted(points); i++)
            {
                if (points[i].Point.GetType() == typeof(SplitRestorePoint))
                {
                    countSplitPoint++;
                }
            }

            if (firstSplitPoint == null && !countSplitPoint.Equals(0))
            {
                throw new BackupsExtraException(
                    "There are no Split Restore Points in the points remaining after the deletion, so you cannot merge");
            }

            for (int i = CountPointsWhichDeleted(points) - 1; i >= 0; i--)
            {
                if (points[i].Point.GetType() == typeof(SplitRestorePoint))
                {
                    firstSplitPoint = new RestorePointWithDateCreation(
                        points[i].NewRepository.MergeTwoPoints(points[i].Point, firstSplitPoint.Point, localKeep), firstSplitPoint.Logging);
                }
                else
                {
                    if (localKeep)
                    {
                        Directory.Delete(
                            points[i].Point.CurrentRepository.Path +
                            points[i].Point.NumberRestorePoint + "_SingleRestorePoint/", true);
                    }
                }
            }

            for (int i = 0; i < CountPointsWhichDeleted(points); i++)
            {
                points.RemoveAt(0);
            }
        }

        public void ExecuteClean(List<RestorePointWithDateCreation> points, bool localKeep)
        {
            for (int i = 0; i < CountPointsWhichDeleted(points); i++)
            {
                if (localKeep)
                {
                    if (points[i].Point.GetType() == typeof(SplitRestorePoint))
                        Directory.Delete(points[i].Point.CurrentRepository.Path + points[i].Point.NumberRestorePoint + "_SplitRestorePoint/", true);
                    if (points[i].Point.GetType() == typeof(SingleRestorePoint))
                        Directory.Delete(points[i].Point.CurrentRepository.Path + points[i].Point.NumberRestorePoint + "_SingleRestorePoint/", true);
                }
                else
                {
                    points.RemoveAt(i);
                }
            }
        }

        private int CountPointsWhichDeleted(List<RestorePointWithDateCreation> points)
        {
            return points.Count - GetPointsAfterClean(points).Count;
        }
    }
}