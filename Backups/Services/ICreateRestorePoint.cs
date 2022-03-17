using System.Collections.Generic;

namespace Backups
{
    public abstract class ICreateRestorePoint
    {
        public abstract IRestorePoint CreateRestorePoint(List<string> directoryFiles, int numberRestorePoint, Repository repository, bool localKeep);
    }
}