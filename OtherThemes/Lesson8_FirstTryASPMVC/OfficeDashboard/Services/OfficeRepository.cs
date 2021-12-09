using System;
using System.Collections.Generic;
using System.Linq;
using OfficeDashboard.Models;

namespace OfficeDashboard.Services
{
    public class OfficeRepository
    {
        private static readonly List<Office> OfficeList;

        static OfficeRepository()
        {
            ModelsGenerator generator = new();
            OfficeList = new List<Office>()
            {
                generator.GenerateOffice(6),
                generator.GenerateOffice(5),
                generator.GenerateOffice(7)
            };
        }

        public IEnumerable<Office> GetOffices() => OfficeList;
        public Office GetOffice(Guid officeId) => OfficeList.FirstOrDefault(o=>o.Id == officeId);
        public IEnumerable<Employee> GetEmployees(Guid officeId) => OfficeList.FirstOrDefault(office => office.Id == officeId)?.Employees;
        public Employee GetEmployee(Guid id) => OfficeList.SelectMany(o => o.Employees).FirstOrDefault(e => e.Id == id);

        public Guid RegisterEmployee(Guid officeId, Employee employee)
        {
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
            if (OfficeList.FirstOrDefault(o => o.Id == officeId) is { } office)
            {
                OfficeList.Remove(office);
                return true;
            }

            return false;
        }

        public bool UpdateEmployeeData(Guid officeId, Employee updated, Guid newOfficeId)
        {
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
            if (OfficeList.FirstOrDefault(o => o.Id == updated.Id) is { } office)
            {
               office.Name = updated.Name;
               return true;
            }

            return false;
        }
    }
}