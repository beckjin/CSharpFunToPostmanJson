using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Parser.CSharp
{
    /// <summary>
    /// 注释xml解析
    /// </summary>
    class CommentHelper
    {
        public static XDocument GetXmlDocument(string xmlStr)
        {
            if (string.IsNullOrWhiteSpace(xmlStr)) { return null; }
            try
            {
                return XDocument.Load(new StringReader(xmlStr));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Parsing XML: " + xmlStr);
                throw ex;
            }
        }

        public static string GetParamByName(XDocument xml, string paramName)
        {
            var val = xml?.Descendants().FirstOrDefault(el =>
            {
                if (el.Name != "param") { return false; }
                var attr = el.Attribute("name");
                return attr != null && attr.Value == paramName;
            });
            return val?.Value.Trim();
        }

        public static string GetSummary(XDocument xml)
        {
            var val = xml?.Descendants().FirstOrDefault(el => el.Name == "summary");
            return val?.Value.Trim();
        }

        public static string GetSummary(string xmlStr)
        {
            return GetSummary(GetXmlDocument(xmlStr));
        }
    }
}
