using System;
using System.Collections.Generic;

namespace OfficeDashboard.Models
{
    public class Office
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}