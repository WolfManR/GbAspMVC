using System.Threading.Tasks;
using RazorEngineCore;

namespace MailTemplates.Razor
{
    public class MailContentBuilder
    {
        private readonly IRazorEngine _razorEngine;

        public MailContentBuilder(IRazorEngine razorEngine)
        {
            _razorEngine = razorEngine;
        }

        public async Task<string> BuildWithTemplate(string template, object model)
        {
            var templateCompiler = await _razorEngine.CompileAsync(template);

            // it make errors
            var mailContent = await templateCompiler.RunAsync(model);

            return mailContent;
        }
    }
}