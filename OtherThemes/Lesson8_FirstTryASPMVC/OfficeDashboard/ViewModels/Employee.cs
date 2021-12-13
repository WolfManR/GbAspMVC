using System;

namespace OfficeDashboard.ViewModels
{
    public class ListEmployee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }

    public class CreateEmployee
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public Guid OfficeId { get; set; }
    }

    public class EditEmployee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Guid OfficeId { get; set; }
    }
}