using System;
using System.Collections.Generic;
using System.Linq;
using Reports.DAL.Entities;
using Reports.Server.Tools;

namespace Reports.Server.Services
{
    public class ReportService : IReportService
    {
        private const int WeekDay = 7;
        private EmployeeService _employeeService;
        private TaskService _taskService;
        private Repository _repository;

        public ReportService()
        {
            _employeeService = new EmployeeService();
            _taskService = new TaskService();
        }
        
        public List<Report> CreateWeeklyReport()
        {
            var resultReport = new List<Report>();
            foreach (TeamLead teamLead in _employeeService.TeamLeads)
            {
                foreach (DirectorEmployee directorEmployee in teamLead.Directors)
                {
                    foreach (Employee employee in directorEmployee.Employees)
                    {
                            employee.SaveNewReportForSomeDays(WeekDay);
                            employee.CloseReport();
                            if (employee.MyReport != null)
                            {
                                resultReport.Add(employee.MyReport);
                            }
                    }
                    directorEmployee.SaveNewReportForSomeDays(WeekDay);
                    directorEmployee.CloseReport();
                    if (directorEmployee.MyReport != null)
                    {
                        resultReport.Add(directorEmployee.MyReport);
                    }
                }
                
                foreach (Employee employee in teamLead.Employees)
                {
                    employee.SaveNewReportForSomeDays(WeekDay);
                    employee.CloseReport();
                    Console.WriteLine(employee.Name);
                    if (employee.MyReport != null)
                    {
                        resultReport.Add(employee.MyReport);
                    }
                }
                teamLead.SaveNewReportForSomeDays(WeekDay);
                teamLead.CloseReport();
                if (teamLead.MyReport != null)
                {
                    resultReport.Add(teamLead.MyReport);
                }
            }

            if (resultReport != null)
            {
                return resultReport;
            }

            throw new ReportServerException("System haven't any report");
        }

        public List<Task> GetWeeklyTask()
        {
            return _taskService.FindTasksForPeriod(WeekDay);
        }

        public List<Report> GetListsResolvedDailyReport()
        {
            var resultReport = new List<Report>();
            foreach (TeamLead teamLead in _employeeService.TeamLeads)
            {
                resultReport = resultReport.Concat(teamLead.GetResolvedReportsSubjects()).ToList();
            }

            if (resultReport != null)
                return resultReport;
            throw new ReportServerException("Result List is null");
        }
        public List<Report> GetListsNotResolvedDailyReport()
        {
            var resultReport = new List<Report>();
            foreach (TeamLead teamLead in _employeeService.TeamLeads)
            {
                resultReport = resultReport.Concat(teamLead.GetNotResolvedReportsSubjects()).ToList();
            }

            if (resultReport != null)
                return resultReport;
            throw new ReportServerException("Result List is null");
        }

        public void AddTaskToReport(Report report, Task task)
        {
            report.AddTask(task);
        }

        public void CloseReport(Report report)
        {
            report.CloseReport();
        }
        

        public TaskService GetTaskService()
        {
            return _taskService;
        }
        public EmployeeService GetEmployeeService()
        {
            return _employeeService;
        }

        public void SetServices(EmployeeService employeeService, TaskService taskService)
        {
            _employeeService = employeeService;
            _taskService = taskService;
        }
    }
}