using System;
using System.Collections.Generic;
using Reports.DAL.Entities;

namespace Reports.Server.Services
{
    public interface IEmployeeService
    {
        TeamLead CreateTeamLead(string name);
        DirectorEmployee CreateDirectorEmployee(TeamLead teamLead, string name);
        Employee CreateEmployeeForDirectorEmployee(DirectorEmployee directorEmployee, string name);
        Employee CreateEmployeeForTeamLead(TeamLead teamLead, string name);
        List<AbstractEmployee> GetEmployeesByFilter(Filter filter);
        AbstractEmployee FindByName(string name);
        AbstractEmployee FindById(Guid id);
        void Delete(Guid id);
        void SerializeTeamLeads(string pathToJson);
        List<TeamLead> DeserializeTeamLeads(string pathToJson);
        Report FindReportById(Guid reportId);
        void Update(AbstractEmployee entity, Guid newDirectorId);

        Repository GetRepository();
    }
}