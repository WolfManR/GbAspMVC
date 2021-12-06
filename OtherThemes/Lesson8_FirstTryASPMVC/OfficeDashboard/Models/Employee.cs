using System;

namespace OfficeDashboard.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public Office Office { get; set; }
    }
}