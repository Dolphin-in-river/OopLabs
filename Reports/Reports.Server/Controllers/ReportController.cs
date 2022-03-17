using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Reports.DAL.Entities;
using Reports.Server.Services;

namespace Reports.Server.Controllers
{
    [ApiController]
    [Route("/reports")]
    public class ReportController : ControllerBase
    {
        private const string PathToStorageTask = "./tasks.json";
        private const string PathToStorageTeamLeads = "./teamLeads.json";
        private ITaskService _taskService = new TaskService();
        private IEmployeeService _employeeService = new EmployeeService();
        private IReportService _reportService;

        public ReportController(IReportService service)
        {
            _reportService = service;
        }

        [HttpGet]
        [Route("/Create-Weekly-Report")]
        public IActionResult CreateWeeklyReport()
        {
            DeserializeTasks();
            DeserializeTeamLeads();
            _reportService.SetServices((EmployeeService) _employeeService, (TaskService) _taskService);
            List<Report> result = _reportService.CreateWeeklyReport();
            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("/Get-Resolved-Daily-Report")]
        public IActionResult GetResolvedDailyReports()
        {
            DeserializeTasks();
            DeserializeTeamLeads();
            _reportService.SetServices((EmployeeService) _employeeService, (TaskService) _taskService);
            List<Report> result = _reportService.GetListsResolvedDailyReport();
            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }
        [HttpGet]
        [Route("/Get-Not-Resolved-Daily-Report")]
        public IActionResult GetNotResolvedDailyReports()
        {
            DeserializeTasks();
            DeserializeTeamLeads();
            _reportService.SetServices((EmployeeService) _employeeService, (TaskService) _taskService);
            List<Report> result = _reportService.GetListsNotResolvedDailyReport();
            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }
        [HttpGet]
        [Route("/Get-Weekly-Tasks")]
        public IActionResult GetWeeklyTasks()
        {
            DeserializeTasks();
            DeserializeTeamLeads();
            _reportService.SetServices((EmployeeService) _employeeService, (TaskService) _taskService);
            List<Task> result = _reportService.GetWeeklyTask();
            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }
        [HttpPut]
        [Route("/Close-Report")]
        public IActionResult CloseReport([FromQuery] Guid reportId)
        {
            DeserializeTeamLeads();
            DeserializeTasks();
            Report report = _employeeService.FindReportById(reportId);
            report.CloseReport();
            if (!report.Open)
            {
                return Ok(report);
            }

            return NotFound();
        }

        [HttpPut]
        [Route("/Add-Task-To-Report")]
        public IActionResult AddTaskToReport([FromQuery] Guid reportId, [FromQuery] Guid taskId)
        {
            DeserializeTeamLeads();
            DeserializeTasks();
            Report report = _employeeService.FindReportById(reportId);
            Task task = _taskService.FindById(taskId);
            report.AddTask(task);
            if (report.Tasks.Contains(task))
            {
                return Ok(report);
            }

            return NotFound();
        }
        private void SerializeTeamLeads()
        {
            _employeeService.SerializeTeamLeads(PathToStorageTeamLeads);
        }

        private void DeserializeTeamLeads()
        {
            _employeeService.DeserializeTeamLeads(PathToStorageTeamLeads);
        }

        private void SerializeTasks()
        {
            _taskService.SerializeTasks(PathToStorageTask);
        }

        private void DeserializeTasks()
        {
            _taskService.DeserializeTasks(PathToStorageTask);
        }
    }
}