using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UI.DataModels;
using UI.Services;

namespace UI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly TemplatesRepository _templatesRepository;

        public IndexModel(TemplatesRepository templatesRepository)
        {
            _templatesRepository = templatesRepository;
        }

        [ViewData] public IReadOnlyCollection<TemplateRecord> Templates { get; set; } 

        public void OnGet()
        {
            Templates = _templatesRepository.Get().ToArray();
        }

        public IActionResult OnPostDelete(string template)
        {
            if (string.IsNullOrEmpty(template)) return Page();

            _templatesRepository.Remove(template);
            return RedirectToPage("Index");
        }

        public IActionResult OnPostEdit(string name, string rawTemplate)
        {
            return RedirectToPage("/Templates/Create", new { templateName = name, usedTemplate = rawTemplate });
        }
    }
}
