using NUnit.Framework;
using Reports.DAL.Entities;
using Reports.Server.Services;

namespace ReportsTest
{
    public class Tests
    {
        private IReportService _reportService;

        [SetUp]
        public void Setup()
        {
            _reportService = new ReportService();
        }

        [Test]
        public void CorrectWorkWithEmployees()
        {
            EmployeeService employeeService = _reportService.GetEmployeeService();
            TeamLead firstTeamLead = employeeService.CreateTeamLead("First Team Lead");
            DirectorEmployee firstDirectorEmployee =
                employeeService.CreateDirectorEmployee(firstTeamLead, "First Director");
            DirectorEmployee secondDirectorEmployee =
                employeeService.CreateDirectorEmployee(firstTeamLead, "Second Director");
            Employee firstEmployee =
                employeeService.CreateEmployeeForDirectorEmployee(firstDirectorEmployee, "First Employee");
            Employee secondEmployee = employeeService.CreateEmployeeForTeamLead(firstTeamLead, "SecondEmployee");
            Assert.AreEqual(1, employeeService.TeamLeads.Count);
            Assert.AreEqual(3, employeeService.FindByName("First Team Lead").GetEmployees().Count);
            Assert.AreEqual("First Team Lead", employeeService.FindById(firstTeamLead.Id).Name);
            employeeService.Delete(firstDirectorEmployee.Id);
            Assert.AreEqual(2, employeeService.FindByName("First Team Lead").GetEmployees().Count);
            employeeService.Update(secondEmployee, secondDirectorEmployee.Id);
            Assert.AreEqual(1, employeeService.FindByName("First Team Lead").GetEmployees().Count);
        }

        [Test]
        public void CorrectWorkWithTasks()
        {
            TaskService taskService = _reportService.GetTaskService();
            EmployeeService employeeService = _reportService.GetEmployeeService();
            TeamLead firstTeamLead = employeeService.CreateTeamLead("First Team Lead");
            Task firstTask = taskService.CreateTask(firstTeamLead);
            Assert.AreEqual(firstTeamLead.Id, taskService.FindById(firstTask.Id).EmployeeId);
            Assert.AreEqual(firstTask.Id, taskService.FindByEmployee(firstTeamLead).Id);
            taskService.AddComment(firstTask, "First comment");
            Assert.AreEqual(1, taskService.FindTaskWithChanges().Count);
            Assert.AreEqual(TaskState.Open, firstTask.State);
            taskService.ChangeTaskState(firstTask, TaskState.Active);
            Assert.AreEqual(TaskState.Active, firstTask.State);
            Employee firstEmployee = employeeService.CreateEmployeeForTeamLead(firstTeamLead, "FirstEmployee");
            Employee secondEmployee = employeeService.CreateEmployeeForTeamLead(firstTeamLead, "SecondEmployee");
            Task secondTask = taskService.CreateTask(firstEmployee);
            Task thirdTask = taskService.CreateTask(secondEmployee);
            Assert.AreEqual(2, taskService.GetListTaskJuniors(firstTeamLead).Count);
        }

        [Test]
        public void CorrectWorkWithReport()
        {
            TaskService taskService = _reportService.GetTaskService();
            EmployeeService employeeService = _reportService.GetEmployeeService();
            TeamLead firstTeamLead = employeeService.CreateTeamLead("First Team Lead");
            Employee firstEmployee = employeeService.CreateEmployeeForTeamLead(firstTeamLead, "FirstEmployee");
            Employee secondEmployee = employeeService.CreateEmployeeForTeamLead(firstTeamLead, "SecondEmployee");
            Task firstTask = taskService.CreateTask(firstTeamLead);
            Task secondTask = taskService.CreateTask(firstEmployee);
            Task thirdTask = taskService.CreateTask(secondEmployee);
            firstEmployee.SaveNewReportForSomeDays(7);
            firstEmployee.CloseReport();
            secondEmployee.SaveNewReportForSomeDays(7);
            secondEmployee.CloseReport();
            Assert.AreEqual(2, firstTeamLead.MakeSprintReport().Count);
            Assert.AreEqual(3, taskService.Tasks.Count);
            Assert.AreEqual(2, firstTeamLead.GetResolvedReportsSubjects().Count);
            Assert.AreEqual(0, firstTeamLead.GetNotResolvedReportsSubjects().Count);
            firstEmployee.SaveNewReportForSomeDays(7);
            firstEmployee.CloseReport();
            Assert.AreEqual(false, firstEmployee.MyReport.Open);
        }

        [Test]
        public void CheckCorrectStorageAtLocalMemory()
        {
            IReportService _reportService = new ReportService();
            TaskService taskService = _reportService.GetTaskService();
            EmployeeService employeeService = _reportService.GetEmployeeService();
            TeamLead firstTeamLead = employeeService.CreateTeamLead("First Team Lead");
            Employee firstEmployee = employeeService.CreateEmployeeForTeamLead(firstTeamLead, "FirstEmployee");
            Employee secondEmployee = employeeService.CreateEmployeeForTeamLead(firstTeamLead, "SecondEmployee");
            Task firstTask = taskService.CreateTask(firstTeamLead);
            Task secondTask = taskService.CreateTask(firstEmployee);
            Task thirdTask = taskService.CreateTask(secondEmployee);
            firstEmployee.SaveNewReportForSomeDays(7);
            firstEmployee.CloseReport();
            secondEmployee.SaveNewReportForSomeDays(7);
            secondEmployee.CloseReport();
            firstEmployee.SaveNewReportForSomeDays(1);
            firstEmployee.CloseReport();
            secondEmployee.SaveNewReportForSomeDays(1);
            firstEmployee.SaveNewReportForSomeDays(7);
            firstEmployee.CloseReport();
            var repository = new Repository();
            repository.SerializeEmployeeService("./reserve_copy.json", employeeService);
            EmployeeService newEmployeeService =
                repository.DeserializeEmployeeService("./reserve_copy.json");
            Assert.AreEqual(employeeService.TeamLeads.Count, newEmployeeService.TeamLeads.Count);
            Assert.AreEqual(employeeService.FindByName("First Team Lead").Id,
                newEmployeeService.FindByName("First Team Lead").Id);
        }
    }
}