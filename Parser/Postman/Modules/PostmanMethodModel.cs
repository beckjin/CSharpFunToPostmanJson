using Parser.CSharp.Modules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Parser.Postman.Modules
{
    public class PostmanMethodModel
    {
        public string ReqTemplate { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public string HttpMethod { get; set; }
        public IEnumerable<ParameterModel> Parameters { get; set; }
        public string Description { get; set; }

        public object GetPostmanMethodObject()
        {
            var parameters = new List<object>();
            foreach (var item in Parameters)
            {
                parameters.Add(new
                {
                    key = item.Name,
                    value = item.DefaultValue ?? string.Empty,
                    description = item.Description ?? string.Empty
                });
            }

            var raw = string.Format(ReqTemplate, ClassName, MethodName);
            var uri = new Uri(raw);
            var localPathParams = uri.LocalPath.Split('/').ToList();
            if (localPathParams.Count > 0) localPathParams.RemoveAt(0);

            var querys = new List<object>();
            if (!string.IsNullOrEmpty(uri.Query))
            {
                var uriQuery = uri.Query.Replace("?", string.Empty);
                var queryParams = uriQuery.Split('&');
                foreach (var item in queryParams)
                {
                    var itemParams = item.Split('=');
                    querys.Add(new
                    {
                        key = itemParams[0],
                        value = itemParams[1],
                        equals = true,
                        description = string.Empty

                    });
                }
            }

            dynamic postmanUrl = new
            {
                raw = raw,
                protocol = uri.Scheme,
                host = new List<string>() { uri.Host }.ToArray(),
                port = uri.Port,
                path = localPathParams.ToArray(),
                query = querys,
                variable = new object[] { }
            };

            var obj = new
            {
                name = Description,
                request = new
                {
                    url = postmanUrl,
                    method = HttpMethod ?? "POST",
                    header = new List<object>() {
                        new {
                            key = "Content-Type",
                            value = "application/x-www-form-urlencoded",
                            description = string.Empty
                        }
                    }.ToArray(),
                    body = new
                    {
                        mode = "urlencoded",
                        urlencoded = parameters
                    },
                    description = string.Empty
                },
                response = new object[] { }
            };
            return obj;
        }
    }
}
