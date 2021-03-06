using System;
using System.Collections.Generic;

namespace OfficeDashboard.Data.Entities
{
    public class OfficeEntity
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public ICollection<EmployeeEntity> Employees { get; init; } = new HashSet<EmployeeEntity>();
    }
}