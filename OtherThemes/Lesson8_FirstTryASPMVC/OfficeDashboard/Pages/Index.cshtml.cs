using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

using OfficeDashboard.Models;
using OfficeDashboard.Services;

using System;
using System.Collections.Generic;
using System.Linq;

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

        public SelectList OfficesSelectList { get; set; }
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

        public IActionResult OnPostDelete(Guid employeeId, Guid officeId)
        {
            if (!ModelState.IsValid) return Page();

            _officeRepository.RemoveEmployee(employeeId);

            return RedirectToPage(new { selectedOffice = officeId });
        }
    }
}
