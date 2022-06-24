using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeDashboard.Services;

using System;
using System.Threading.Tasks;
using OfficeDashboard.ViewModels;

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
        public EditOffice Office { get; set; }

        public async Task OnGetAsync(Guid id)
        {
            Office = await _officeRepository.GetOfficeForEdit(id);
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();

            await _officeRepository.UpdateOfficeData(Office);

            return RedirectToPage("../Index", new { selectedOffice = Office.Id });
        }

        public IActionResult OnPostGoBack()
        {
            return RedirectToPage("../Index", new { selectedOffice = Office.Id });
        }
    }
}
