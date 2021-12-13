using System;
using System.Collections.Generic;

namespace OfficeDashboard.ViewModels
{
    public class Office
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<ListEmployee> Employees { get; set; } = new HashSet<ListEmployee>();
    }

    public class OfficeSelectListItem
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
    }

    public class EditOffice
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}