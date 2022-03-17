using System;

namespace Reports.DAL.Entities
{
    public class TaskChanges
    {
        public string Comment
        {
            get;
            set;
        }

        public DateTime DateChanges
        {
            get;
            set;
        }

        public TaskState State
        {
            get;
            set;
        }

        public Guid EmployeeId
        {
            get;
            set;
        }

        public TaskChanges()
        {
            DateChanges = DateTime.Today;
        }
    }
}