using System.Collections.Generic;

namespace Parser.CSharp.Modules
{
    public class MethodModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string HttpMethod { get; set; }
        public IEnumerable<ParameterModel> Parameters { get; set; }
    }
}
