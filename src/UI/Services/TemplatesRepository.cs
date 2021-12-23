using System.Collections.Generic;
using System.Linq;
using UI.DataModels;

namespace UI.Services
{
    public class TemplatesRepository
    {
        private static List<TemplateRecord> _templates;

        public TemplatesRepository() => _templates ??= new List<TemplateRecord>();

        public TemplateRecord? Get(string name) => _templates.FirstOrDefault(t => t.Name == name);
        public void Save(TemplateRecord templateRecord) => _templates.Add(templateRecord);
    }
}