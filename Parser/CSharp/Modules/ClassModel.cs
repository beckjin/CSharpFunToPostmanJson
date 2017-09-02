using System.Collections.Generic;

namespace Parser.CSharp.Modules
{
    public class ClassModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<MethodModel> Methods { get; set; }
    }
}
