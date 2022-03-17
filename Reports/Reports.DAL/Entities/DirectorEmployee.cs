using System;
using System.Collections.Generic;
using Reports.DAL.Tools;

namespace Reports.DAL.Entities
{
    public class DirectorEmployee : AbstractEmployee
    {
        public List<Employee> Employees
        {
            get;
            set;
        }

        public Guid TeamLeadId
        {
            get;
            set;
        }

        public DirectorEmployee()
            : base()
        {
        }
        public DirectorEmployee(Guid id, string name)
            : base(id, name)
        {
            Type = EmployeeType.DirectorEmployee;
            Employees = new List<Employee>();
        }

        public override void DeleteById(Guid id)
        {
            foreach (Employee item in Employees)
            {
                if (item.Id.Equals(id))
                {
                    Employees.Remove(item);
                    return;
                }
            }

            throw new ReportsDalExceptions("This person hasn't been deleted");
        }

        public override void AddEmployee(AbstractEmployee employee)
        {
            if (employee.Type.Equals(EmployeeType.Employee))
            {
                Employees.Add((Employee) employee);
            }
            else
            {
                throw new ReportsDalExceptions("I can add only simple employee");
            }
        }

        public override Guid GetDirectorId()
        {
            return TeamLeadId;
        }

        public override void SetDirectorId(Guid newId)
        {
            TeamLeadId = newId;
        }

        public override List<AbstractEmployee> GetEmployees()
        {
            var resultList = new List<AbstractEmployee>();
            foreach (Employee item in Employees)
            {
                resultList.Add(item);
            }

            return resultList;
        }

        public override List<Report> GetResolvedReportsSubjects()
        {
            var resultReport = new List<Report>();
            foreach (Employee item in Employees)
            {
                if (!item.MyReport.Open)
                {
                    resultReport.Add(item.MyReport);
                }
            }

            return resultReport;
        }

        public override List<Report> GetNotResolvedReportsSubjects()
        {
            var resultReport = new List<Report>();
            foreach (Employee item in Employees)
            {
                if (item.MyReport.Open)
                {
                    resultReport.Add(item.MyReport);
                }
            }

            return resultReport;
        }
    }
}