using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using OfficeDashboard.Models;

using OfficeDashboard.Services;

using System;

namespace OfficeDashboard.Pages.Offices
{
    public class EditModel : PageModel
    {
        private readonly OfficeRepository _officeRepository;

        public EditModel(OfficeRepository officeRepository)
        {
            _officeRepository = officeRepository;
        }

        [BindProperty]
        public Office Office { get; set; }

        public void OnGet(Guid id)
        {
            Office = _officeRepository.GetOffice(id);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            _officeRepository.UpdateOfficeData(Office);

            return RedirectToPage("../Index", new { selectedOffice = Office.Id });
        }

        public IActionResult OnPostGoBack()
        {
            return RedirectToPage("../Index", new { selectedOffice = Office.Id });
        }
    }
}
