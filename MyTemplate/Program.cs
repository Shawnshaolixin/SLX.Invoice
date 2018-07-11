using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTemplate
{
    class Program
    {
        static void Main(string[] args)
        {
            var name = "shao li xin ";
            //这里要注意一点。 就是 html的渲染是 流量器的工作。你看
                MyRender("", "hello.html", name);//原html
       
            Console.ReadKey();
        }
        static void MyRender(string request,string name,string json)
        { //你假设这个目录就是 template 目录
          var streamReader=  File.OpenText(name);
      


          var html=  streamReader.ReadToEnd();
            streamReader.Close();
            var wenHtml= html.Replace("{{name}}", json);

            File.WriteAllText(name, wenHtml);

        }
    }
   
}
