using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeDashboard.Models;
using OfficeDashboard.Services;

namespace OfficeDashboard.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly OfficeRepository _officeRepository;

        public IndexModel(ILogger<IndexModel> logger, OfficeRepository officeRepository)
        {
            _logger = logger;
            _officeRepository = officeRepository;
        }

        [BindProperty] 
        public string CurrentOffice { get; set; }
        public List<Office> Offices;
        
        public List<Employee> Employees;

        public void OnGet()
        {
            UpdatePageData();
        }

        public SelectListItem[] GetOffices()
        {
            return Offices.Select(o => new SelectListItem(o.Name, o.Id.ToString())).ToArray();
        }

        public IActionResult OnPost()
        {
            if(!ModelState.IsValid) return Page();

            if (Guid.TryParse(CurrentOffice, out var officeId))
            {
                Employees = _officeRepository.GetEmployees(officeId).ToList();
                UpdatePageData();
            }

            return Page();
        }

        private void UpdatePageData()
        {
            Offices = new List<Office>(_officeRepository.GetOffices());
        }
    }
}
