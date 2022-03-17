using System;
using System.Collections.Generic;
using System.IO;
using Backups;
using BackupsExtra.Logging;
using BackupsExtra.Tools;
using Newtonsoft.Json;

namespace BackupsExtra
{
    public class BackupsExtraService : IBackupExtraService
    {
        private List<ExtraBackupJob> _extraBackupJobs;

        public BackupsExtraService()
        {
            _extraBackupJobs = new List<ExtraBackupJob>();
        }

        public ExtraBackupJob AddExtraBackupJob(List<string> file, ICreateRestorePoint point, string path, bool localKeep, AbstractLogging logging)
        {
            var newExtraBackupJob = new ExtraBackupJob(file, point, path, localKeep, logging);
            _extraBackupJobs.Add(newExtraBackupJob);
            return newExtraBackupJob;
        }

        public ExtraBackupJob DeserializeExtraBackupJob(string pathToJson)
        {
            if (!File.Exists(pathToJson))
            {
                throw new BackupsExtraException("This file doesn't exists");
            }

            CheckNullPath(pathToJson);
            var streamReader = new StreamReader(pathToJson);
            string text = streamReader.ReadToEnd();
            ExtraBackupJob newExtraBackupJob = JsonConvert.DeserializeObject<ExtraBackupJob>(text, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    NullValueHandling = NullValueHandling.Ignore,
                });

            _extraBackupJobs.Add(newExtraBackupJob);
            newExtraBackupJob.Logging.ExecuteLogging(GetDeserializeMessageForLogging(newExtraBackupJob.Logging.Configuration));
            return newExtraBackupJob;
        }

        public void SerializeExtraBackupJob(string pathToJson, ExtraBackupJob extraBackupJob)
        {
            if (File.Exists(pathToJson))
            {
                throw new BackupsExtraException("This file has already exists");
            }

            CheckNullPath(pathToJson);
            string output = JsonConvert.SerializeObject(extraBackupJob, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
            });
            var fileStream = new FileStream(pathToJson, FileMode.Append);
            byte[] array = System.Text.Encoding.Default.GetBytes(output);
            fileStream.Write(array, 0, array.Length);
            fileStream.Dispose();
            extraBackupJob.Logging.ExecuteLogging(GetSerializeMessageForLogging(extraBackupJob.Logging.Configuration));
        }

        public List<ExtraBackupJob> GetExtraBackupJobs()
        {
            return _extraBackupJobs;
        }

        private void CheckNullPath(string path)
        {
            if (path == null)
            {
                throw new BackupsExtraException("Path cannot be null");
            }
        }

        private string GetSerializeMessageForLogging(bool loggingConfiguration)
        {
            string text = null;
            if (loggingConfiguration)
            {
                text += DateTime.Now + ", ";
            }

            text += "Serialize Extra Backup Job" + Environment.NewLine;
            return text;
        }

        private string GetDeserializeMessageForLogging(bool loggingConfiguration)
        {
            string text = null;
            if (loggingConfiguration)
            {
                text += DateTime.Now + ", ";
            }

            text += "Deserialize Extra Backup Job" + Environment.NewLine;
            return text;
        }
    }
}