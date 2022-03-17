using System;
using System.Collections.Generic;

namespace Reports.DAL.Entities
{
    public class Report
    {
        public List<Task> Tasks
        {
            get;
            set;
        }

        public DateTime DateOpen
        {
            get;
            set;
        }

        public bool Open
        {
            get;
            set;
        }

        public Guid Id
        {
            get;
            init;
        }
        public Report()
        {
        }
        public Report(List<Task> tasks, int days)
        {
            var tasksInThisDays = new List<Task>();
            foreach (Task item in tasks)
            {
                if (item.CreationData.Day + days >= DateTime.Now.Day ||
                    item.LastDateChanges.Day + days >= DateTime.Now.Day)
                {
                    tasksInThisDays.Add(item);
                }
            }

            DateOpen = DateTime.Now;
            Tasks = tasksInThisDays;
            Open = true;
            Id = Guid.NewGuid();
        }

        public void AddTask(Task task)
        {
            if (!Open)
            {
                throw new Exception("This report has been resolved. You can't make changes");
            }

            Tasks.Add(task);
        }

        public void CloseReport()
        {
            Open = false;
        }
    }
}