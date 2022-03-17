using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups;
using BackupsExtra.Logging;
using BackupsExtra.Tools;

namespace BackupsExtra
{
    public class RepositoryExtra
    {
        public RepositoryExtra()
        {
        }

        public RepositoryExtra(AbstractLogging logging)
        {
            Logging = logging;
        }

        public AbstractLogging Logging
        {
            get;
            set;
        }

        public void RecoverSplitPoint(IRecovery recoveryInfo, IRestorePoint point)
        {
            var listRecoveryZipFiles = new Dictionary<string, string>();
            string pathToRestorePoint =
                point.CurrentRepository.Path + point.NumberRestorePoint + "_SplitRestorePoint/";
            CheckExistDirectoryToZipArchive(pathToRestorePoint);
            if (recoveryInfo.GetTypeRecovery() == TypeRecovery.OriginalLocation)
            {
                foreach (string currentDirectory in Directory.GetFiles(pathToRestorePoint))
                {
                    foreach (string oldDirectory in point.DirectoryFiles)
                    {
                        if ((GetName(oldDirectory) + ".zip").Equals(GetName(currentDirectory)))
                        {
                            listRecoveryZipFiles.Add(currentDirectory, oldDirectory);
                        }
                    }
                }
            }

            if (recoveryInfo.GetTypeRecovery() == TypeRecovery.DifferentLocation)
            {
                listRecoveryZipFiles = GetTableForRecoveryToDifferentLocation(recoveryInfo, pathToRestorePoint);
            }

            Logging.ExecuteLogging(GetMessageForLogging(listRecoveryZipFiles));
            RecoveryFilesFromSplitRestore(listRecoveryZipFiles);

            Directory.Delete(point.CurrentRepository.Path + point.NumberRestorePoint + "_SplitRestorePoint/", true);
        }

        public void RecoverSinglePoint(IRecovery recoveryInfo, IRestorePoint point)
        {
            var listRecoveryZipFiles = new Dictionary<string, string>();
            string pathToRestorePoint =
                point.CurrentRepository.Path + point.NumberRestorePoint + "_SingleRestorePoint/";
            string newPath;
            CheckExistDirectoryToZipArchive(pathToRestorePoint);

            newPath = point.CurrentRepository.Path + "BufferDirectoryForSplitArchive/";
            Directory.CreateDirectory(newPath);
            ZipFile.ExtractToDirectory(pathToRestorePoint + point.NumberRestorePoint + ".zip", newPath, true);

            if (recoveryInfo.GetTypeRecovery() == TypeRecovery.OriginalLocation)
            {
                foreach (string currentDirectory in Directory.GetFiles(newPath))
                {
                    foreach (string oldDirectory in point.DirectoryFiles)
                    {
                        if ((point.NumberRestorePoint + "_" + GetName(oldDirectory)).Equals(GetName(currentDirectory)))
                        {
                            listRecoveryZipFiles.Add(currentDirectory, oldDirectory);
                        }
                    }
                }
            }

            if (recoveryInfo.GetTypeRecovery() == TypeRecovery.DifferentLocation)
            {
                listRecoveryZipFiles = GetTableForRecoveryToDifferentLocation(recoveryInfo, newPath);
            }

            Logging.ExecuteLogging(GetMessageForLogging(listRecoveryZipFiles));
            RecoveryFilesFromSingleRestore(listRecoveryZipFiles);
            Directory.Delete(newPath, true);

            Directory.Delete(point.CurrentRepository.Path + point.NumberRestorePoint + "_SingleRestorePoint/", true);
        }

        public IRestorePoint MergeTwoPoints(IRestorePoint firstPoint, IRestorePoint secondPoint, bool localKeep)
        {
            bool findDirectory = false;
            var missingDirectories = new List<string>();
            foreach (string directoryInFirstPoint in firstPoint.DirectoryFiles)
            {
                foreach (string directoryInSecondPoint in secondPoint.DirectoryFiles)
                {
                    if (directoryInFirstPoint == directoryInSecondPoint)
                    {
                        findDirectory = true;
                        break;
                    }
                }

                if (!findDirectory)
                {
                    missingDirectories.Add(directoryInFirstPoint);
                    if (localKeep)
                    {
                        string filePath = firstPoint.CurrentRepository.Path + firstPoint.NumberRestorePoint +
                                          "_SplitRestorePoint/" +
                                          GetName(directoryInFirstPoint) + ".zip";
                        string newPath = secondPoint.CurrentRepository.Path + secondPoint.NumberRestorePoint +
                                         "_SplitRestorePoint/" + GetName(directoryInFirstPoint) + ".zip";
                        var fileInf = new FileInfo(filePath);

                        if (fileInf.Exists)
                        {
                            fileInf.CopyTo(newPath, true);
                        }
                    }
                }

                findDirectory = false;
            }

            foreach (string item in missingDirectories)
            {
                secondPoint.DirectoryFiles.Add(item);
            }

            if (localKeep)
            {
                Directory.Delete(firstPoint.CurrentRepository.Path + firstPoint.NumberRestorePoint + "_SplitRestorePoint/", true);
            }

            return secondPoint;
        }

        private string GetName(string directoryName)
        {
            string name = null;
            for (int i = directoryName.LastIndexOf("/") + 1; i < directoryName.Length; i++)
            {
                name += directoryName[i];
            }

            return name;
        }

        private Dictionary<string, string> GetTableForRecoveryToDifferentLocation(IRecovery recoveryInfo, string pathToRestorePoint)
        {
            var listRecoveryZipFiles = new Dictionary<string, string>();
            var newRecoveryInfo = (RecoveryToDifferentLocation)recoveryInfo;
            foreach (string currentDirectory in Directory.GetFiles(pathToRestorePoint))
            {
                listRecoveryZipFiles.Add(currentDirectory, newRecoveryInfo.NewLocation);
            }

            return listRecoveryZipFiles;
        }

        private string GetNameDirectory(string directoryName)
        {
            string name = null;
            for (int i = 0; i < directoryName.LastIndexOf("/") + 1; i++)
            {
                name += directoryName[i];
            }

            return name;
        }

        private void CheckExistDirectoryToZipArchive(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new BackupsExtraException("The archive doesn't exists");
            }
        }

        private void RecoveryFilesFromSingleRestore(Dictionary<string, string> listRecoveryZipFiles)
        {
            foreach (KeyValuePair<string, string> item in listRecoveryZipFiles)
            {
                var fileInf = new FileInfo(item.Value);

                if (fileInf.Exists)
                {
                    File.Delete(item.Value);
                }

                var directoryInfo = new DirectoryInfo(GetNameDirectory(item.Value));
                if (!directoryInfo.Exists)
                {
                    Directory.CreateDirectory(GetNameDirectory(item.Value));
                }

                File.Move(item.Key, GetNameDirectory(item.Value) + GetName(item.Key), true);
            }
        }

        private void RecoveryFilesFromSplitRestore(Dictionary<string, string> listRecoveryZipFiles)
        {
            foreach (KeyValuePair<string, string> item in listRecoveryZipFiles)
            {
                var fileInf = new FileInfo(item.Value);

                if (fileInf.Exists)
                {
                    File.Delete(item.Value);
                }

                ZipFile.ExtractToDirectory(item.Key, GetNameDirectory(item.Value));
            }
        }

        private string GetMessageForLogging(Dictionary<string, string> listRecoveryFiles)
        {
            string message = null;
            if (listRecoveryFiles.Count == 0)
            {
                throw new ArgumentNullException();
            }

            if (Logging.Configuration)
            {
                message += DateTime.Now + ", " + Environment.NewLine;
            }

            foreach (KeyValuePair<string, string> item in listRecoveryFiles)
            {
                message += item.Key + " has been restored to " + item.Value + Environment.NewLine;
            }

            return message;
        }
    }
}