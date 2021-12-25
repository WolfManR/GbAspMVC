using System.Linq;
using MailTemplates.Razor;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using System.Threading.Tasks;
using UI.DataModels;
using UI.Services;

namespace UI.Pages.Templates
{
    public class CreateModel : PageModel
    {
        private readonly ModelsGenerator _modelsGenerator;
        private readonly MailContentBuilder _mailContentBuilder;
        private readonly TemplatesRepository _templatesRepository;

        public CreateModel(ModelsGenerator modelsGenerator, MailContentBuilder mailContentBuilder, TemplatesRepository templatesRepository)
        {
            _modelsGenerator = modelsGenerator;
            _mailContentBuilder = mailContentBuilder;
            _templatesRepository = templatesRepository;
        }

        [ViewData] public SelectList ModelTypes { get; set; }
        [BindProperty] public string ModelType { get; set; }
        [BindProperty] public string Name { get; set; }
        [BindProperty] public string Template { get; set; }
        [TempData] public string Preview { get; set; }

        public void OnGet(string templateName, string preview, string usedTemplate, string selectedModelType)
        {
            ModelTypes = new SelectList(
                new[]
                {
                    new {type=nameof(Book), name = nameof(Book)},
                },
                "type",
                "name");
            ModelType = selectedModelType;
            Template = usedTemplate;
            Preview = preview;
            Name = templateName;
        }

        public async Task<IActionResult> OnPostPreview()
        {
            if (string.IsNullOrWhiteSpace(Template)) return Page();

            object data = ModelType switch
                          {
                              "Book" => _modelsGenerator.GenerateBooks().First()
                          };

            var preview = await _mailContentBuilder.BuildWithTemplate(Template, data);

            return RedirectToPage(new { templateName = Name, preview, usedTemplate = Template, selectedModelType = ModelType });
        }

        public async Task<IActionResult> OnPostSave()
        {
            if (string.IsNullOrEmpty(Template) || string.IsNullOrEmpty(Name)) return Page();

            _templatesRepository.Save(new TemplateRecord()
            {
                Name = Name,
                RawTemplate = Template
            });

            return RedirectToPage("../Index");
        }
    }
}
