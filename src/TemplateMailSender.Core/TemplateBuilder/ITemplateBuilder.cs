using System.Threading.Tasks;

namespace TemplateMailSender.Core.TemplateBuilder
{
    public interface ITemplateBuilder
    {
        Task<string> Build(string template, object model);
    }
}