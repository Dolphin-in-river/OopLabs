using System;
using System.Collections.Generic;

namespace Reports.DAL.Entities
{
    public abstract class AbstractEmployee
    {
        protected const int OneDay = 1;
        public Guid Id
        {
            get;
            set;
        }

        public Report MyReport
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public EmployeeType Type
        {
            get;
            set;
        }

        public List<Task> Tasks
        {
            get;
            set;
        }

        public AbstractEmployee()
        {
        }
        public AbstractEmployee(Guid id, string name)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id), "Id is invalid");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "Name is invalid");
            }

            Tasks = new List<Task>();
            Id = id;
            Name = name;
        }

        public abstract Guid GetDirectorId();
        public abstract void SetDirectorId(Guid newId);

        public abstract void DeleteById(Guid id);
        public abstract void AddEmployee(AbstractEmployee employee);

        public abstract List<AbstractEmployee> GetEmployees();

        public void AddTask(Task task)
        {
            Tasks.Add(task);
        }

        public abstract List<Report> GetResolvedReportsSubjects();
        public abstract List<Report> GetNotResolvedReportsSubjects();

        public void SaveNewReportForSomeDays(int days)
        {
            if (MyReport == null)
                MyReport = new Report(Tasks, days);
            else
            {
                if (MyReport.Open)
                    MyReport = new Report(Tasks, days);
            }
        }

        public void CloseReport()
        {
            MyReport.CloseReport();
        }
    }
}