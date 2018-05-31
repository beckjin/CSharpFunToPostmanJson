using System;
using System.Diagnostics;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            var reqTemplate = "https://{{host}}/ajaxpage/AjaxHeader.ashx?controller={0}&action={1}";
            var projectPath = @"C:\Users\Administrator\Desktop\MD.Web.Ajax\MD.Web.Ajax.csproj";
            var outputDir = @"C:\Users\Administrator\Desktop\mdWebInterface";

            Console.WriteLine("开始生成...");
            var sw = new Stopwatch();
            sw.Start();

            var parser = new MingdaoParser(projectPath, outputDir, reqTemplate);
            parser.Output();

            sw.Stop();
            Console.WriteLine("总耗时：" + sw.ElapsedMilliseconds + "毫秒");
            Console.WriteLine("按任意键结束...");
            Console.ReadKey();
        }
    }
}
