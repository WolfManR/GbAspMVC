using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

using OfficeDashboard.Models;
using OfficeDashboard.Services;

using System;

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
        public Guid CurrentOfficeId { get; set; }

        [BindProperty]
        public Employee NewEmployee { get; set; }

        [ViewData]
        public SelectList OfficesSelectList { get; set; }

        [ViewData]
        public Office SelectedOffice { get; set; }

        public void OnGet(Guid selectedOffice)
        {
            OfficesSelectList = new SelectList(_officeRepository.GetOffices(), "Id", "Name");
            if (selectedOffice != Guid.Empty)
            {
                CurrentOfficeId = selectedOffice;
                SelectedOffice = _officeRepository.GetOffice(selectedOffice);
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            return RedirectToPage(new { selectedOffice = CurrentOfficeId });
        }

        public IActionResult OnPostCreate(Guid officeId)
        {
            if (!ModelState.IsValid) return Page();

            _officeRepository.RegisterEmployee(officeId, NewEmployee);

            return RedirectToPage(new { selectedOffice = officeId });
        }

        public IActionResult OnPostDelete(Guid employeeId, Guid officeId)
        {
            _officeRepository.RemoveEmployee(employeeId);

            return RedirectToPage(new { selectedOffice = officeId });
        }

        public IActionResult OnPostEdit(Guid employeeId)
        {
            return RedirectToPage("/Employees/Edit", new { id = employeeId });
        }

        public IActionResult OnPostRemoveOffice()
        {
            _officeRepository.RemoveOffice(CurrentOfficeId);

            return RedirectToPage(new { selectedOffice = CurrentOfficeId });
        }

        public IActionResult OnPostEditOffice()
        {
            return RedirectToPage("/Offices/Edit", new { id = CurrentOfficeId });
        }
    }
}
