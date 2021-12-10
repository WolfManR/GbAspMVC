using System;
using System.Collections.Generic;
using System.Linq;
using OfficeDashboard.Models;

namespace OfficeDashboard.Services
{
    public class OfficeRepository
    {
        private static readonly List<Office> OfficeList;
        private static readonly Office OutOfBorderEmployees;
        static OfficeRepository()
        {
            ModelsGenerator generator = new();
            OfficeList = new List<Office>()
            {
                generator.GenerateOffice(6),
                generator.GenerateOffice(5),
                generator.GenerateOffice(7)
            };

            OutOfBorderEmployees = new Office() { Id = Guid.NewGuid(), Name = "Out of border employees" };
        }

        public IEnumerable<Office> GetOffices()
        {
            List<Office> offices = new List<Office>(OfficeList);
            if(OutOfBorderEmployees.Employees.Count > 0) offices.Add(OutOfBorderEmployees);
            return offices;
        }

        public Office GetOffice(Guid officeId)
        {
            if (IsOutOfBorderEmployeesOffice(officeId)) return OutOfBorderEmployees;
            return OfficeList.FirstOrDefault(o => o.Id == officeId);
        }

        public IEnumerable<Employee> GetEmployees(Guid officeId)
        {
            if (IsOutOfBorderEmployeesOffice(officeId)) return OutOfBorderEmployees.Employees;
            return OfficeList.FirstOrDefault(office => office.Id == officeId)?.Employees;
        }

        public Employee GetEmployee(Guid id) => GetOffices().SelectMany(o => o.Employees).FirstOrDefault(e => e.Id == id);

        public Guid RegisterEmployee(Guid officeId, Employee employee)
        {
            if (IsOutOfBorderEmployeesOffice(officeId)) return Guid.Empty;

            if (OfficeList.FirstOrDefault(o => o.Id == officeId) is { } office)
            {
                var id = Guid.NewGuid();
                employee.Id = id;
                office.Employees.Add(employee);
                employee.Office = office;
                return id;
            }

            return Guid.Empty;
        }

        public bool RemoveEmployee(Guid employeeId)
        {
            var employee = OfficeList.SelectMany(o => o.Employees).FirstOrDefault(e => e.Id == employeeId);
            if (employee is null) return false;

            employee.Office.Employees.Remove(employee);
            employee.Office = null;
            return true;
        }

        public bool RemoveOffice(Guid officeId)
        {
            if (IsOutOfBorderEmployeesOffice(officeId)) return false;

            if (OfficeList.FirstOrDefault(o => o.Id == officeId) is { } office)
            {
                OfficeList.Remove(office);
                if (office.Employees.Count > 0)
                {
                    foreach (var employee in office.Employees)
                    {
                        office.Employees.Remove(employee);
                        OutOfBorderEmployees.Employees.Add(employee);
                        employee.Office = OutOfBorderEmployees;
                    }
                }
                return true;
            }

            return false;
        }

        public bool UpdateEmployeeData(Guid officeId, Employee updated, Guid newOfficeId)
        {
            if (IsOutOfBorderEmployeesOffice(newOfficeId)) return false;

            if (OfficeList.FirstOrDefault(o => o.Id == officeId) is { } office)
            {
                if (office.Employees.FirstOrDefault(e => e.Id == updated.Id) is { } employee)
                {
                    employee.Name = updated.Name;
                    employee.Surname = updated.Surname;
                    if (newOfficeId != Guid.Empty && newOfficeId != officeId && OfficeList.FirstOrDefault(o => o.Id == newOfficeId) is { } newOffice)
                    {
                        employee.Office = newOffice;
                        newOffice.Employees.Add(employee);
                        office.Employees.Remove(employee);
                    }
                    return true;
                }
            }

            return false;
        }

        public bool UpdateOfficeData(Office updated)
        {
            if (IsOutOfBorderEmployeesOffice(updated.Id)) return false;

            if (OfficeList.FirstOrDefault(o => o.Id == updated.Id) is { } office)
            {
               office.Name = updated.Name;
               return true;
            }

            return false;
        }

        private bool IsOutOfBorderEmployeesOffice(Guid officeId) => officeId != Guid.Empty && OutOfBorderEmployees.Id == officeId;
    }
}