using System.Collections.Generic;

namespace Backups.Services
{
    public class CreateSingleRestorePoint : ICreateRestorePoint
    {
        public override IRestorePoint CreateRestorePoint(List<string> directoryFiles, int numberRestorePoint, Repository repository, bool localKeep)
        {
            return new SingleRestorePoint(directoryFiles, numberRestorePoint, repository, localKeep);
        }
    }
}