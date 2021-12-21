using MailTemplates.Razor;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using System.Threading.Tasks;

using UI.Services;

namespace UI.Pages.Templates
{
    public class CreateModel : PageModel
    {
        private readonly ModelsGenerator _modelsGenerator;
        private readonly MailContentBuilder _mailContentBuilder;

        public CreateModel(ModelsGenerator modelsGenerator, MailContentBuilder mailContentBuilder)
        {
            _modelsGenerator = modelsGenerator;
            _mailContentBuilder = mailContentBuilder;
        }
        [ViewData] public SelectList ModelTypes { get; set; }
        [BindProperty] public string ModelType { get; set; }
        [BindProperty] public string Template { get; set; }
        [TempData] public string Preview { get; set; }

        public void OnGet(string preview, string usedTemplate, string selectedModelType)
        {
            ModelTypes = new SelectList(
                new[]
                {
                    (type: "Book", name: "Book"),
                },
                "type",
                "name");
            ModelType = selectedModelType;
            Template = usedTemplate;
            Preview = preview;
        }

        public async Task<IActionResult> OnPostPreview()
        {
            if (string.IsNullOrWhiteSpace(Template)) return Page();

            var preview = await _mailContentBuilder.BuildWithTemplate(Template, _modelsGenerator.GenerateBooks(3));

            return RedirectToPage(new { preview, template = Template });
        }
    }
}
