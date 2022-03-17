using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Reports.DAL.Entities;
using Reports.Server.Services;

namespace Reports.Server.Controllers
{
    [ApiController]
    [Route("/tasks")]
    public class TaskController : ControllerBase
    {
        private const string PathToStorageTask = "./tasks.json";
        private const string PathToStorageTeamLeads = "./teamLeads.json";
        private ITaskService _service;
        private IEmployeeService _employeeService = new EmployeeService();

        public TaskController(ITaskService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("/Create-Task")]
        public Task CreateTask([FromQuery] string nameEmployee, [FromQuery] Guid idEmployee)
        {
            DeserializeTasks();
            DeserializeTeamLeads();
            Task newTask = null;
            if (nameEmployee != null)
            {
                newTask = _service.CreateTask(_employeeService.FindByName(nameEmployee));
            }

            if (idEmployee != Guid.Empty)
            {
                newTask = _service.CreateTask(_employeeService.FindById(idEmployee));
            }

            SerializeTeamLeads();
            SerializeTasks();
            return newTask;
        }


        [HttpPut]
        [Route("/Change-State")]
        public Task ChangeState([FromQuery] Guid id, [FromQuery] TaskState newState)
        {
            DeserializeTasks();
            DeserializeTeamLeads();
            _service.ChangeTaskState(_service.FindById(id), newState);
            SerializeTeamLeads();
            SerializeTasks();
            return _service.FindById(id);
        }

        [HttpPut]
        [Route("/Add-Comment")]
        public Task AddComment([FromQuery] Guid id, [FromQuery] string comment)
        {
            DeserializeTasks();
            DeserializeTeamLeads();
            _service.AddComment(_service.FindById(id), comment);
            SerializeTeamLeads();
            SerializeTasks();
            return _service.FindById(id);
        }

        [HttpPut]
        [Route("/Change-Employee")]
        public Task ChangeEmployee([FromQuery] Guid taskId, [FromQuery] Guid employeeId)
        {
            DeserializeTasks();
            DeserializeTeamLeads();
            _service.ChangeEmployee(_service.FindById(taskId), employeeId);
            SerializeTeamLeads();
            SerializeTasks();
            return _service.FindById(taskId);
        }

        [HttpGet]
        [Route("/Get-All-Tasks")]
        public IActionResult GetAllTask()
        {
            DeserializeTasks();
            DeserializeTeamLeads();
            List<Task> result = _service.GetAllTasks();
            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("/Get-Task-By-Id")]
        public IActionResult GetTaskById([FromQuery] Guid taskId)
        {
            DeserializeTasks();
            DeserializeTeamLeads();
            Task result = _service.FindById(taskId);
            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("/Get-Task-By-Date-Creation")]
        public IActionResult GetTaskByDateCreation([FromQuery] DateTime dateTimeCreation)
        {
            DeserializeTasks();
            DeserializeTeamLeads();
            Task result = _service.FindByDateCreation(dateTimeCreation);
            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("/Get-Task-By-Date-Changes")]
        public IActionResult GetTaskByDateChanges([FromQuery] DateTime dateTimeChanges)
        {
            DeserializeTasks();
            DeserializeTeamLeads();
            Task result = _service.FindByDateLastChanges(dateTimeChanges);
            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("/Get-Task-By-Employee-Id")]
        public IActionResult GetTaskByEmployeeId([FromQuery] Guid employeeId)
        {
            DeserializeTasks();
            DeserializeTeamLeads();
            Task result = _service.FindByEmployee(_employeeService.FindById(employeeId));
            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("/Get-Task-With-Changes")]
        public IActionResult GetTaskWithChanges()
        {
            DeserializeTasks();
            DeserializeTeamLeads();
            List<Task> result = _service.FindTaskWithChanges();
            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("/Get-List-Task-Juniors")]
        public IActionResult GetListTaskJuniors([FromQuery] Guid employeeId)
        {
            DeserializeTasks();
            DeserializeTeamLeads();
            List<Task> result = _service.GetListTaskJuniors(_employeeService.FindById(employeeId));
            if (result != null)
            {
                return Ok(result);
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
            _service.SerializeTasks(PathToStorageTask);
        }

        private void DeserializeTasks()
        {
            _service.DeserializeTasks(PathToStorageTask);
        }
    }
}