using System;
using System.Collections.Generic;
using Reports.DAL.Tools;

namespace Reports.DAL.Entities
{
    public class Employee : AbstractEmployee
    {
        public Guid DirectorId
        {
            get;
            set;
        }

        public Employee()
        {
        }
        public Employee(Guid id, string name)
            : base(id, name)
        {
            Type = EmployeeType.Employee;
        }

        public override void DeleteById(Guid id)
        {
            throw new ReportsDalExceptions("Employee haven't any employees");
        }

        public override void AddEmployee(AbstractEmployee employee)
        {
            throw new ReportsDalExceptions("This person can not add new subjects");
        }

        public override Guid GetDirectorId()
        {
            return DirectorId;
        }

        public override void SetDirectorId(Guid newId)
        {
            DirectorId = newId;
        }

        public override List<AbstractEmployee> GetEmployees()
        {
            throw new ReportsDalExceptions("Employee haven't any subjects");
        }

        public override List<Report> GetResolvedReportsSubjects()
        {
            throw new ReportsDalExceptions("This employee hasn't any subjects");
        }

        public override List<Report> GetNotResolvedReportsSubjects()
        {
            throw new ReportsDalExceptions("This employee hasn't any subjects");
        }
    }
}