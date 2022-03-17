using System;
using System.Collections.Generic;
using Reports.DAL.Entities;

namespace Reports.Server.Services
{
    public interface ITaskService
    {
        Task FindById(Guid id);
        List<Task> GetAllTasks();
        Task FindByDateCreation(DateTime days);
        Task FindByDateLastChanges(DateTime days);
        List<Task> FindTasksForPeriod(int days);
        Task FindByEmployee(AbstractEmployee employee);
        List<Task> FindTaskWithChanges();
        Task CreateTask(AbstractEmployee employee);
        Task ChangeTaskState(Task task, TaskState newState);
        Task AddComment(Task task, string comment);
        Task ChangeEmployee(Task task, Guid id);
        List<Task> GetListTaskJuniors(AbstractEmployee newEmployee);
        void DeserializeTasks(string pathToJson);
        void SerializeTasks(string pathToJson);
    }
}