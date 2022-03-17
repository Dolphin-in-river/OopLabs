using System.Collections.Generic;
using Reports.DAL.Entities;

namespace Reports.Server.Services
{
    public interface IReportService
    {
        TaskService GetTaskService();
        List<Report> CreateWeeklyReport();
        List<Task> GetWeeklyTask();
        List<Report> GetListsResolvedDailyReport();
        List<Report> GetListsNotResolvedDailyReport();
        void AddTaskToReport(Report report, Task task);
        void CloseReport(Report report);
        EmployeeService GetEmployeeService();
        void SetServices(EmployeeService employeeService, TaskService taskService);
    }
}