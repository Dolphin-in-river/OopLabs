using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Reports.DAL.Entities;
using Reports.Server.Services;
using Reports.Server.Tools;

namespace Reports.Server.Controllers
{
    [ApiController]
    [Route("/employees")]
    public class EmployeeController : ControllerBase
    {
        private const string PathToStorage = "./teamLeads.json";
        private IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }


        [HttpPost]
        [Route("/Create-Team-Lead")]
        public TeamLead CreateTeamLead([FromQuery] string name)
        {
            TeamLead newTeamLead = _service.CreateTeamLead(name);
            SerializeTeamLeads();
            return newTeamLead;
        }

        [HttpPost]
        [Route("/Create-Director")]
        public DirectorEmployee CreateDirector([FromQuery] string nameTeamLead, [FromQuery] Guid idTeamLead,
            [FromQuery] string name)
        {
            DeserializeTeamLeads();
            if (!string.IsNullOrWhiteSpace(nameTeamLead))
            {
                AbstractEmployee resultOfFind = _service.FindByName(nameTeamLead);
                if (resultOfFind == null)
                {
                    throw new ReportServerException("TeamLead hasn't been founded");
                }

                if (resultOfFind.Type == EmployeeType.TeamLead)
                {
                    DirectorEmployee result = _service.CreateDirectorEmployee((TeamLead) resultOfFind, name);
                    SerializeTeamLeads();
                    return result;
                }
            }

            if (idTeamLead != Guid.Empty)
            {
                AbstractEmployee result = _service.FindById(idTeamLead);
                if (result == null)
                {
                    throw new ReportServerException("TeamLead hasn't been founded");
                }

                if (result.Type == EmployeeType.TeamLead)
                {
                    DirectorEmployee returnResult = _service.CreateDirectorEmployee((TeamLead) result, name);
                    SerializeTeamLeads();
                    return returnResult;
                }
            }

            throw new ReportServerException("Can't found this TeamLead");
        }

        [HttpPost]
        [Route("/Create-Employee")]
        public Employee CreateEmployee([FromQuery] string nameTeamLead, [FromQuery] Guid idTeamLead,
            [FromQuery] string nameDirector, [FromQuery] Guid idDirector,
            [FromQuery] string name)
        {
            DeserializeTeamLeads();
            if (!string.IsNullOrWhiteSpace(nameTeamLead))
            {
                AbstractEmployee result = _service.FindByName(nameTeamLead);
                if (result == null)
                {
                    throw new ReportServerException("TeamLead hasn't been founded");
                }

                if (result.Type == EmployeeType.TeamLead)
                {
                    Employee returnResult = _service.CreateEmployeeForTeamLead((TeamLead) result, name);
                    SerializeTeamLeads();
                    return returnResult;
                }

                throw new ReportServerException("This person isn't teamLead");
            }

            if (idTeamLead != Guid.Empty)
            {
                AbstractEmployee result = _service.FindById(idTeamLead);
                if (result == null)
                {
                    throw new ReportServerException("TeamLead hasn't been founded");
                }

                if (result.Type == EmployeeType.TeamLead)
                {
                    Employee returnResult = _service.CreateEmployeeForTeamLead((TeamLead) result, name);
                    SerializeTeamLeads();
                    return returnResult;
                }

                throw new ReportServerException("This person isn't teamLead");
            }

            if (!string.IsNullOrWhiteSpace(nameDirector))
            {
                AbstractEmployee result = _service.FindByName(nameDirector);
                if (result == null)
                {
                    throw new ReportServerException("Director hasn't been founded");
                }

                if (result.Type == EmployeeType.DirectorEmployee)
                {
                    Employee returnResult = _service.CreateEmployeeForDirectorEmployee((DirectorEmployee) result, name);
                    SerializeTeamLeads();
                    return returnResult;
                }

                throw new ReportServerException("This person isn't director");
            }

            if (idDirector != Guid.Empty)
            {
                AbstractEmployee result = _service.FindById(idDirector);
                if (result == null)
                {
                    throw new ReportServerException("Director hasn't been founded");
                }

                if (result.Type == EmployeeType.DirectorEmployee)
                {
                    Employee returnResult = _service.CreateEmployeeForDirectorEmployee((DirectorEmployee) result, name);
                    SerializeTeamLeads();
                    return returnResult;
                }

                throw new ReportServerException("This person isn't director");
            }

            throw new Exception("Can't create new Employee");
        }

        [HttpPut]
        [Route("/Update-Employee")]
        public void Update([FromQuery] string nameEmployee, [FromQuery] Guid idEmployee, [FromQuery] Guid idDirector)
        {
            DeserializeTeamLeads();
            if (idEmployee != Guid.Empty)
            {
                _service.Update(_service.FindById(idEmployee), idDirector);
            }

            if (nameEmployee != null)
            {
                _service.Update(_service.FindByName(nameEmployee), idDirector);
            }

            SerializeTeamLeads();
        }

        [HttpDelete]
        [Route("/Delete-Employee")]
        public void Delete([FromQuery] Guid id)
        {
            DeserializeTeamLeads();
            _service.Delete(id);
            SerializeTeamLeads();
        }

        [HttpGet]
        [Route("/Get-Employee")]
        public IActionResult Find([FromQuery] string name, [FromQuery] Guid id)
        {
            DeserializeTeamLeads();
            if (!string.IsNullOrWhiteSpace(name))
            {
                AbstractEmployee result = _service.FindByName(name);
                if (result != null)
                {
                    return Ok(result);
                }

                return NotFound();
            }

            if (id != Guid.Empty)
            {
                AbstractEmployee result = _service.FindById(id);
                if (result != null)
                {
                    return Ok(result);
                }

                return NotFound();
            }

            return StatusCode((int) HttpStatusCode.BadRequest);
        }

        [HttpGet]
        [Route("/Get-Employee-With-Filter")]
        public IActionResult FindWithFilter([FromQuery] EmployeeType type1, [FromQuery] EmployeeType type2,
            [FromQuery] EmployeeType type3)
        {
            DeserializeTeamLeads();
            var filter = new Filter();
            if (type1 != null)
            {
                filter.AddType(type1);
            }
            if (type2 != null)
            {
                filter.AddType(type2);
            }
            if (type3 != null)
            {
                filter.AddType(type3);
            }

            List<AbstractEmployee> result = _service.GetEmployeesByFilter(filter);
            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        private void SerializeTeamLeads()
        {
            _service.SerializeTeamLeads(PathToStorage);
        }

        private void DeserializeTeamLeads()
        {
            _service.DeserializeTeamLeads(PathToStorage);
        }
    }
}