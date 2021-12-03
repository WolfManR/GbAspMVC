using System;
using System.Collections.Generic;

namespace OfficeDashboard.Models
{
    public class Office
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Employ> Employees { get; set; } = new HashSet<Employ>();
    }
}