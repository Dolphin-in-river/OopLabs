using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Reports.DAL.Entities;
using Reports.Server.Controllers;
using Reports.Server.Tools;

namespace Reports.Server.Services
{
    public class EmployeeService : IEmployeeService
    {
        public List<TeamLead> TeamLeads
        {
            get;
            set;
        }

        public Repository NewRepository
        {
            get;
        }

        public EmployeeService()
        {
            TeamLeads = new List<TeamLead>();
            NewRepository = new Repository();
        }

        public TeamLead CreateTeamLead(string name)
        {
            var newTeamLead = new TeamLead(Guid.NewGuid(), name);
            TeamLeads.Add(newTeamLead);
            return newTeamLead;
        }

        public DirectorEmployee CreateDirectorEmployee(TeamLead teamLead, string name)
        {
            var newDirectorEmployee = new DirectorEmployee(Guid.NewGuid(), name);
            teamLead.Directors.Add(newDirectorEmployee);
            newDirectorEmployee.TeamLeadId = teamLead.Id;
            return newDirectorEmployee;
        }

        public Employee CreateEmployeeForDirectorEmployee(DirectorEmployee directorEmployee, string name)
        {
            var newEmployee = new Employee(Guid.NewGuid(), name);
            newEmployee.DirectorId = directorEmployee.Id;
            directorEmployee.Employees.Add(newEmployee);
            return newEmployee;
        }

        public Employee CreateEmployeeForTeamLead(TeamLead teamLead, string name)
        {
            var newEmployee = new Employee(Guid.NewGuid(), name);
            newEmployee.DirectorId = teamLead.Id;
            teamLead.Employees.Add(newEmployee);
            return newEmployee;
        }

        public List<AbstractEmployee> GetEmployeesByFilter(Filter filter)
        {
            var resultList = new List<AbstractEmployee>();
            foreach (TeamLead teamLead in TeamLeads)
            {
                if (filter.Types.Contains(teamLead.Type))
                {
                    resultList.Add(teamLead);
                }

                foreach (DirectorEmployee directorEmployee in teamLead.Directors)
                {
                    if (filter.Types.Contains(directorEmployee.Type))
                    {
                        resultList.Add(directorEmployee);
                    }

                    if (directorEmployee.Type == EmployeeType.DirectorEmployee)
                    {
                        foreach (Employee employee in directorEmployee.Employees)
                        {
                            if (filter.Types.Contains(employee.Type))
                            {
                                resultList.Add(employee);
                            }
                        }
                    }

                    foreach (Employee employee in teamLead.Employees)
                    {
                        if (filter.Types.Contains(employee.Type))
                        {
                            resultList.Add(employee);
                        }
                    }
                }
            }

            return resultList;
        }


        public AbstractEmployee FindByName(string name)
        {
            foreach (TeamLead teamLead in TeamLeads)
            {
                if (teamLead.Name.Equals(name))
                {
                    return teamLead;
                }

                foreach (DirectorEmployee directorEmployee in teamLead.Directors)
                {
                    if (directorEmployee.Name.Equals(name))
                    {
                        return directorEmployee;
                    }

                    if (directorEmployee.Type == EmployeeType.DirectorEmployee)
                    {
                        foreach (Employee employee in directorEmployee.Employees)
                        {
                            if (employee.Name.Equals(name))
                            {
                                return employee;
                            }
                        }
                    }
                }

                foreach (Employee employee in teamLead.Employees)
                {
                    if (employee.Name.Equals(name))
                    {
                        return employee;
                    }
                }
            }

            return null;
        }

        public AbstractEmployee FindById(Guid id)
        {
            foreach (TeamLead teamLead in TeamLeads)
            {
                if (teamLead.Id.Equals(id))
                {
                    return teamLead;
                }

                foreach (DirectorEmployee directorEmployee in teamLead.Directors)
                {
                    if (directorEmployee.Id.Equals(id))
                    {
                        return directorEmployee;
                    }

                    if (directorEmployee.Type == EmployeeType.DirectorEmployee)
                    {
                        foreach (Employee employee in directorEmployee.Employees)
                        {
                            if (employee.Id.Equals(id))
                            {
                                return employee;
                            }
                        }
                    }
                }

                foreach (Employee employee in teamLead.Employees)
                {
                    if (employee.Id.Equals(id))
                    {
                        return employee;
                    }
                }
            }

            return null;
        }

        public Report FindReportById(Guid reportId)
        {
            foreach (var teamLead in TeamLeads)
            {
                if (teamLead.MyReport.Id == reportId)
                {
                    return teamLead.MyReport;
                }

                foreach (DirectorEmployee directorEmployee in teamLead.Directors)
                {
                    if (directorEmployee.MyReport.Id.Equals(reportId))
                    {
                        return directorEmployee.MyReport;
                    }

                    if (directorEmployee.Type == EmployeeType.DirectorEmployee)
                    {
                        foreach (Employee employee in directorEmployee.Employees)
                        {
                            if (employee.MyReport.Id.Equals(reportId))
                            {
                                return employee.MyReport;
                            }
                        }
                    }
                }

                foreach (Employee employee in teamLead.Employees)
                {
                    if (employee.MyReport.Id.Equals(reportId))
                    {
                        return employee.MyReport;
                    }
                }
            }

            throw new Exception("This Report hasn't been founded");
        }

        public void Delete(Guid id)
        {
            foreach (TeamLead teamLead in TeamLeads)
            {
                if (teamLead.Id.Equals(id))
                {
                    TeamLeads.Remove(teamLead);
                    return;
                }

                foreach (DirectorEmployee directorEmployee in teamLead.Directors)
                {
                    if (directorEmployee.Id.Equals(id))
                    {
                        teamLead.Directors.Remove(directorEmployee);
                        return;
                    }

                    if (directorEmployee.Type == EmployeeType.DirectorEmployee)
                    {
                        foreach (Employee employee in directorEmployee.Employees)
                        {
                            if (employee.Id.Equals(id))
                            {
                                directorEmployee.Employees.Remove(employee);
                                return;
                            }
                        }
                    }
                }

                foreach (Employee employee in teamLead.Employees)
                {
                    if (employee.Id.Equals(id))
                    {
                        teamLead.Employees.Remove(employee);
                        return;
                    }
                }
            }

            throw new Exception("This Id wasn't found");
        }

        public void Update(AbstractEmployee entity, Guid newDirectorId)
        {
            AbstractEmployee lastDirector = FindById(entity.GetDirectorId());
            lastDirector.DeleteById(entity.Id);
            entity.SetDirectorId(newDirectorId);
            AbstractEmployee newDirector = FindById(newDirectorId);
            newDirector.AddEmployee(entity);
        }


        public void SerializeTeamLeads(string pathToJson)
        {
            if (File.Exists(pathToJson))
            {
                File.Delete(pathToJson);
            }

            CheckNullPath(pathToJson);
            string output = JsonConvert.SerializeObject(TeamLeads, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
            });
            var fileStream = new FileStream(pathToJson, FileMode.Append);
            byte[] array = System.Text.Encoding.Default.GetBytes(output);
            fileStream.Write(array, 0, array.Length);
            fileStream.Dispose();
        }

        public List<TeamLead> DeserializeTeamLeads(string pathToJson)
        {
            if (!File.Exists(pathToJson))
            {
                throw new ReportServerException("This file doesn't exists");
            }

            CheckNullPath(pathToJson);
            var streamReader = new StreamReader(pathToJson);
            string text = streamReader.ReadToEnd();
            streamReader.Dispose();
            List<TeamLead> newTeamLeads = JsonConvert.DeserializeObject<List<TeamLead>>(text,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    NullValueHandling = NullValueHandling.Ignore,
                });

            TeamLeads = newTeamLeads;
            return newTeamLeads;
        }

        private void CheckNullPath(string path)
        {
            if (path == null)
            {
                throw new ReportServerException("Path cannot be null");
            }
        }

        public Repository GetRepository()
        {
            return NewRepository;
        }
    }
}