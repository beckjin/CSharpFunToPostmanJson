using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Newtonsoft.Json;
using Parser.CSharp;
using Parser.Postman.Modules;
using System.IO;
using System.Linq;
using System.Text;

namespace Parser
{
    public class MingdaoParser
    {
        private readonly Project project;
        private readonly string outputDir;
        private readonly string reqTemplate;

        public MingdaoParser(string projectPath, string outputDir, string reqTemplate)
        {
            var workspace = MSBuildWorkspace.Create();
            project = workspace.OpenProjectAsync(projectPath).GetAwaiter().GetResult();
            this.reqTemplate = reqTemplate;
            this.outputDir = outputDir;
        }

        public void Output()
        {
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            foreach (var f in Directory.GetFiles(outputDir))
            {
                File.Delete(f);
            }

            foreach (var doc in project.Documents.Where(doc => doc.Folders.Any()))
            {
                if (doc.Folders.First().Equals("Controller") && doc.Name.EndsWith("Controller.cs"))
                {
                    var sm = doc.GetSemanticModelAsync().GetAwaiter().GetResult();
                    var models = new ClassParser(sm).Parse();
                    foreach (var model in models)
                    {
                        var poatmanModel = new PostmanFileModel()
                        {
                            Name = model.Description,
                            ClassName = model.Name.Replace("Controller", string.Empty),
                            ReqTemplate = reqTemplate,
                            Items = model.Methods
                        };
                        using (var writer = new StreamWriter(Path.Combine(outputDir, (doc.Name[0].ToString().ToLower() + doc.Name.Substring(1)).Replace("Controller.cs", ".json")), true, Encoding.UTF8))
                        {
                            writer.Write(JsonConvert.SerializeObject(poatmanModel.GetPostmanObject()));
                        }
                    }
                }
            }
        }
    }
}
