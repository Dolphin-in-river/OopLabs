using System.Collections.Generic;

namespace Backups
{
    public class CreateSplitRestorePoint : ICreateRestorePoint
    {
        public override IRestorePoint CreateRestorePoint(List<string> directoryFiles, int numberRestorePoint, Repository repository, bool localKeep)
        {
            return new SplitRestorePoint(directoryFiles, numberRestorePoint, repository, localKeep);
        }
    }
}