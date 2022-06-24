using System.Threading.Tasks;
using RazorEngineCore;
using TemplateMailSender.Core.TemplateBuilder;

namespace MailTemplates.Razor
{
    public class MailContentBuilder : ITemplateBuilder
    {
        private readonly IRazorEngine _razorEngine;

        public MailContentBuilder(IRazorEngine razorEngine)
        {
            _razorEngine = razorEngine;
        }

        public async Task<string> Build(string template, object model)
        {
            var templateCompiler = await _razorEngine.CompileAsync(template);

            // it make errors
            var mailContent = await templateCompiler.RunAsync(model);

            return mailContent;
        }
    }
}