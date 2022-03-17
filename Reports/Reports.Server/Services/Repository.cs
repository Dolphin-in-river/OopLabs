using System.IO;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Newtonsoft.Json;
using Reports.Server.Tools;

namespace Reports.Server.Services
{
    public class Repository
    {
        public Repository()
        {
        }

        public EmployeeService DeserializeEmployeeService(string pathToJson)
        {
            if (!File.Exists(pathToJson))
            {
                throw new ReportServerException("This file doesn't exists");
            }

            CheckNullPath(pathToJson);
            var streamReader = new StreamReader(pathToJson);
            string text = streamReader.ReadToEnd();
            EmployeeService newEmployeeService = JsonConvert.DeserializeObject<EmployeeService>(text,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    NullValueHandling = NullValueHandling.Ignore,
                });

            return newEmployeeService;
        }

        public void SerializeEmployeeService(string pathToJson, EmployeeService employeeService)
        {
            if (File.Exists(pathToJson))
            {
                File.Delete(pathToJson);
            }

            CheckNullPath(pathToJson);
            string output = JsonConvert.SerializeObject(employeeService, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
            });
            var fileStream = new FileStream(pathToJson, FileMode.Append);
            byte[] array = System.Text.Encoding.Default.GetBytes(output);
            fileStream.Write(array, 0, array.Length);
            fileStream.Dispose();
        }

        public TaskService DeserializeTaskService(string pathToJson)
        {
            if (!File.Exists(pathToJson))
            {
                throw new ReportServerException("This file doesn't exists");
            }

            CheckNullPath(pathToJson);
            var streamReader = new StreamReader(pathToJson);
            string text = streamReader.ReadToEnd();
            TaskService newTaskService = JsonConvert.DeserializeObject<TaskService>(text,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    NullValueHandling = NullValueHandling.Ignore,
                });

            return newTaskService;
        }

        public void SerializeTaskService(string pathToJson, TaskService taskService)
        {
            if (File.Exists(pathToJson))
            {
                throw new ReportServerException("This file has already exists");
            }

            CheckNullPath(pathToJson);
            string output = JsonConvert.SerializeObject(taskService, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
            });
            var fileStream = new FileStream(pathToJson, FileMode.Append);
            byte[] array = System.Text.Encoding.Default.GetBytes(output);
            fileStream.Write(array, 0, array.Length);
            fileStream.Dispose();
        }

        private void CheckNullPath(string path)
        {
            if (path == null)
            {
                throw new ReportServerException("Path cannot be null");
            }
        }
    }
}