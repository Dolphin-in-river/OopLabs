using System;
using System.Collections.Generic;

namespace Reports.DAL.Entities
{
    public class Task
    {
        public Guid Id
        {
            get;
            set;
        }

        public Guid EmployeeId
        {
            get;
            set;
        }

        public List<TaskChanges> Changes
        {
            get;
            set;
        }

        public DateTime CreationData
        {
            get;
            set;
        }

        public DateTime LastDateChanges
        {
            get;
            set;
        }

        public TaskState State
        {
            get;
            set;
        }

        public Task()
        {
        }
        public Task(AbstractEmployee employee)
        {
            CreationData = DateTime.Today;
            State = TaskState.Open;
            Id = Guid.NewGuid();
            EmployeeId = employee.Id;
            Changes = new List<TaskChanges>();
        }

        public Task AddComment(string comment)
        {
            var taskChangesBuilder = new TaskChangesBuilder();
            LastDateChanges = DateTime.Now;
            TaskChanges changes = taskChangesBuilder.BuildComment(comment).GetResult();
            Changes.Add(changes);
            return this;
        }

        public void ChangeEmployee(Guid id)
        {
            var taskChangesBuilder = new TaskChangesBuilder();
            EmployeeId = id;
            LastDateChanges = DateTime.Now;
            Changes.Add(taskChangesBuilder.BuildNewEmployee(id).GetResult());
        }

        public Task ChangeState(TaskState newState)
        {
            var taskChangesBuilder = new TaskChangesBuilder();
            State = newState;
            LastDateChanges = DateTime.Now;
            Changes.Add(taskChangesBuilder.BuildNewTaskState(newState).GetResult());
            return this;
        }

        public Guid GetIdEmployee()
        {
            return EmployeeId;
        }
    }
}