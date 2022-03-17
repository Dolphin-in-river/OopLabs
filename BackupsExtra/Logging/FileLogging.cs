using System.IO;

namespace BackupsExtra.Logging
{
    public class FileLogging : AbstractLogging
    {
        public FileLogging(string filePath)
        {
            FilePath = filePath;
            TypeLogging = TypeLogging.FileLogging;
        }

        public string FilePath
        {
            get;
        }

        public override void ExecuteLogging(string message)
        {
            var fileStream = new FileStream(FilePath, FileMode.Append);
            byte[] array = System.Text.Encoding.Default.GetBytes(message);
            fileStream.Write(array, 0, array.Length);
            fileStream.Dispose();
        }
    }
}