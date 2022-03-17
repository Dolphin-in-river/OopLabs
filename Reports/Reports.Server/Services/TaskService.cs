using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Reports.DAL.Entities;
using Reports.Server.Tools;

namespace Reports.Server.Services
{
    public class TaskService : ITaskService
    {
        public List<Task> Tasks
        {
            get;
            set;
        }

        public TaskService()
        {
            Tasks = new List<Task>();
        }

        public List<Task> GetAllTasks()
        {
            return Tasks;
        }
        public Task FindById(Guid id)
        {
            foreach (Task item in Tasks)
            {
                if (item.Id.Equals(id))
                {
                    return item;
                }
            }

            return null;
        }

        public Task FindByDateCreation(DateTime days)
        {
            foreach (Task item in Tasks)
            {
                if (item.CreationData.Equals(days))
                {
                    return item;
                }
            }

            return null;
        }

        public Task FindByDateLastChanges(DateTime days)
        {
            foreach (Task item in Tasks)
            {
                if (item.LastDateChanges.Equals(days))
                {
                    return item;
                }
            }

            return null;
        }

        public List<Task> FindTasksForPeriod(int days)
        {
            var resultList = new List<Task>();
            foreach (Task item in Tasks)
            {
                if (DateTime.Now.Day < days + item.CreationData.Day)
                {
                    resultList.Add(item);
                }
            }

            return resultList;
        }

        public Task FindByEmployee(AbstractEmployee employee)
        {
            foreach (Task item in Tasks)
            {
                if (item.EmployeeId.Equals(employee.Id))
                {
                    return item;
                }
            }

            return null;
        }

        public List<Task> FindTaskWithChanges()
        {
            var resultList = new List<Task>();
            foreach (Task item in Tasks)
            {
                if (item.Changes.Count != 0)
                {
                    resultList.Add(item);
                }
            }

            return resultList;
        }

        public Task CreateTask(AbstractEmployee employee)
        {
            var newTask = new Task(employee);
            employee.AddTask(newTask);
            Tasks.Add(newTask);
            return newTask;
        }

        public Task ChangeTaskState(Task task, TaskState newState)
        {
            return task.ChangeState(newState);
        }

        public Task AddComment(Task task, string comment)
        {
            return task.AddComment(comment);
        }

        public Task ChangeEmployee(Task task, Guid id)
        {
            task.ChangeEmployee(id);
            return task;
        }

        public List<Task> GetListTaskJuniors(AbstractEmployee newEmployee)
        {
            var resultList = new List<Task>();
            foreach (AbstractEmployee item in newEmployee.GetEmployees())
            {
                foreach (Task task in item.Tasks)
                {
                    resultList.Add(task);
                }
            }

            if (resultList.Count == 0)
            {
                throw new Exception("Everyone hasn't got task");
            }

            return resultList;
        }
        

        public void SerializeTasks(string pathToJson)
        {
            if (File.Exists(pathToJson))
            {
                File.Delete(pathToJson);
            }

            CheckNullPath(pathToJson);
            string output = JsonConvert.SerializeObject(Tasks, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
            });
            var fileStream = new FileStream(pathToJson, FileMode.Append);
            byte[] array = System.Text.Encoding.Default.GetBytes(output);
            fileStream.Write(array, 0, array.Length);
            fileStream.Dispose();
        }

        public void DeserializeTasks(string pathToJson)
        {
            if (!File.Exists(pathToJson))
            {
                return;
            }

            CheckNullPath(pathToJson);
            var streamReader = new StreamReader(pathToJson);
            string text = streamReader.ReadToEnd();
            streamReader.Dispose();
            List<Task> newTaskService = JsonConvert.DeserializeObject<List<Task>>(text,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    NullValueHandling = NullValueHandling.Ignore,
                });

            Tasks = newTaskService;
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