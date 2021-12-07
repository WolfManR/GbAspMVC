using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeDashboard.Models;
using OfficeDashboard.Services;

namespace OfficeDashboard.Pages.Employees
{
    public class EditModel : PageModel
    {
        private readonly OfficeRepository _officeRepository;

        public EditModel(OfficeRepository officeRepository)
        {
            _officeRepository = officeRepository;
        }

        [BindProperty] public Employee Employee { get; set; }

        public void OnGet(Guid id)
        {
           Employee = _officeRepository.GetEmployee(id);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            _officeRepository.UpdateEmployeeData(Employee.Office.Id, Employee);

            return RedirectToPage("../Index", new { selectedOffice = Employee.Office.Id });
        }

        public IActionResult OnPostGoBack()
        {
            return RedirectToPage("../Index", new { selectedOffice = Employee?.Office.Id });
        }
    }
}
