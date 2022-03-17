using System;
using System.Collections.Generic;
using Reports.DAL.Tools;

namespace Reports.DAL.Entities
{
    public class TeamLead : AbstractEmployee
    {
        public List<DirectorEmployee> Directors
        {
            get;
            set;
        }

        public List<Employee> Employees
        {
            get;
            set;
        }

        public TeamLead()
        {
            
        }
        public TeamLead(Guid id, string name)
            : base(id, name)
        {
            Type = EmployeeType.TeamLead;
            Directors = new List<DirectorEmployee>();
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

            foreach (DirectorEmployee item in Directors)
            {
                if (item.Id.Equals(id))
                {
                    Directors.Remove(item);
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
                return;
            }

            if (employee.Type.Equals(EmployeeType.DirectorEmployee))
            {
                Directors.Add((DirectorEmployee) employee);
                return;
            }

            throw new ReportsDalExceptions("I can't add this employee");
        }

        public override Guid GetDirectorId()
        {
            throw new ReportsDalExceptions("This person hasn't Director Id");
        }

        public override void SetDirectorId(Guid newId)
        {
            throw new ReportsDalExceptions("This person hasn't Director Id");
        }

        public override List<AbstractEmployee> GetEmployees()
        {
            var resultList = new List<AbstractEmployee>();
            foreach (Employee item in Employees)
            {
                resultList.Add(item);
            }

            foreach (DirectorEmployee item in Directors)
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
                item.SaveNewReportForSomeDays(OneDay);
                if (!item.MyReport.Open)
                {
                    resultReport.Add(item.MyReport);
                }
            }

            foreach (DirectorEmployee item in Directors)
            {
                foreach (Employee employee in item.Employees)
                {
                    employee.SaveNewReportForSomeDays(OneDay);
                    if (!employee.MyReport.Open)
                    {
                        resultReport.Add(employee.MyReport);
                    }
                }
                item.SaveNewReportForSomeDays(OneDay);
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
            foreach (var item in Employees)
            {
                item.SaveNewReportForSomeDays(OneDay);
                if (item.MyReport.Open)
                {
                    resultReport.Add(item.MyReport);
                }
            }

            foreach (DirectorEmployee item in Directors)
            {
                foreach (Employee employee in item.Employees)
                {
                    employee.SaveNewReportForSomeDays(OneDay);
                    if (employee.MyReport.Open)
                    {
                        resultReport.Add(employee.MyReport);
                    }
                }
                item.SaveNewReportForSomeDays(OneDay);
                if (item.MyReport.Open)
                {
                    resultReport.Add(item.MyReport);
                }
            }

            return resultReport;
        }

        public List<Report> MakeSprintReport()
        {
            var resultReport = new List<Report>();
            foreach (Employee item in Employees)
            {
                if (!item.MyReport.Open)
                {
                    resultReport.Add(item.MyReport);
                }
            }

            foreach (DirectorEmployee item in Directors)
            {
                if (!item.MyReport.Open)
                {
                    resultReport.Add(item.MyReport);
                }
            }

            return resultReport;
        }
    }
}