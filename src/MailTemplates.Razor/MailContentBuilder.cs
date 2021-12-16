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

            var mailContent = await templateCompiler.RunAsync(model);

            return mailContent;
        }
    }

    public class MailContentBuilder<TModel>
    {
        private readonly IRazorEngine _razorEngine;

        public MailContentBuilder(IRazorEngine razorEngine)
        {
            _razorEngine = razorEngine;
        }

        public async Task<string> BuildWithTemplate(string template, TModel model)
        {
            var templateCompiler = await _razorEngine.CompileAsync<RazorEngineTemplateBase<TModel>>(template);

            var mailContent = await templateCompiler.RunAsync(instance => instance.Model = model);

            return mailContent;
        }
    }
}