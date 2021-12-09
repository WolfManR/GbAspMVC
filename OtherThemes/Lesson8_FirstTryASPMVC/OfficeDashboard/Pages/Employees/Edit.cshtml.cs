using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using OfficeDashboard.Models;
using OfficeDashboard.Services;

using System;

namespace OfficeDashboard.Pages.Employees
{
    public class EditModel : PageModel
    {
        private readonly OfficeRepository _officeRepository;

        public EditModel(OfficeRepository officeRepository)
        {
            _officeRepository = officeRepository;
        }

        [ViewData]
        public SelectList OfficesSelectList { get; set; }

        [BindProperty]
        public Guid OfficeId { get; set; }

        [BindProperty]
        public Employee Employee { get; set; }

        public void OnGet(Guid id)
        {
            Employee = _officeRepository.GetEmployee(id);
            OfficeId = Employee.Office.Id;
            OfficesSelectList = new SelectList(_officeRepository.GetOffices(), "Id", "Name");
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            _officeRepository.UpdateEmployeeData(Employee.Office.Id, Employee, OfficeId);

            return RedirectToPage("../Index", new { selectedOffice = Employee.Office.Id });
        }

        public IActionResult OnPostGoBack()
        {
            return RedirectToPage("../Index", new { selectedOffice = Employee?.Office.Id });
        }
    }
}
