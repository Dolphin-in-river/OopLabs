using System;
using System.IO;
using Backups;
using BackupsExtra.Logging;

namespace BackupsExtra
{
    public class RestorePointWithDateCreation
    {
        public RestorePointWithDateCreation(IRestorePoint point, AbstractLogging logging)
        {
            Point = point;
            Logging = logging;
            NewRepository = new RepositoryExtra(logging);
        }

        public IRestorePoint Point
        {
            get;
            set;
        }

        public AbstractLogging Logging
        {
            get;
            set;
        }

        public RepositoryExtra NewRepository
        {
            get;
            set;
        }

        public void RecoverSplitPoint(IRecovery recoveryInfo)
        {
            NewRepository.RecoverSplitPoint(recoveryInfo, Point);
        }

        public void RecoverSinglePoint(IRecovery recoveryInfo)
        {
            NewRepository.RecoverSinglePoint(recoveryInfo, Point);
        }

        public string GetMessageForLogging()
        {
            string text = null;
            if (Logging.Configuration)
            {
                text += Point.CreateDataTime + ", ";
            }

            text += Point.GetType() + " has been created" + Environment.NewLine;
            return text;
        }
    }
}