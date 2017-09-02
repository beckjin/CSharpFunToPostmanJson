using Parser.CSharp.Modules;
using System.Collections.Generic;

namespace Parser.Postman.Modules
{
    public class PostmanFileModel
    {
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string ReqTemplate { get; set; }
        public IEnumerable<MethodModel> Items { get; set; }

        public object GetPostmanObject()
        {
            var items = new List<object>();
            foreach (var item in Items)
            {
                var model = new PostmanMethodModel()
                {
                    ReqTemplate = ReqTemplate,
                    ClassName = ClassName,
                    MethodName = item.Name,
                    HttpMethod = item.HttpMethod,
                    Parameters = item.Parameters,
                    Description = item.Description
                };
                items.Add(model.GetPostmanMethodObject());
            }

            var obj = new
            {
                info = new
                {
                    name = Name,
                    description = string.Empty,
                    schema = "https://schema.getpostman.com/json/collection/v2.0.0/collection.json"
                },
                item = items
            };
            return obj;
        }


    }
}
