using Consul;
using System;
using System.Linq;

namespace ConsulTest
{
    class Program
    {
        static void Main(string[] args)
        {

            Uri uri = new Uri(" http://apiservice1/api/values ");

            using (var consulClient = new ConsulClient(c => c.Address = new Uri("Http://127.0.0.1:8500")))
            {
                var services = consulClient.Agent.Services().Result.Response.Values
                    .Where(p => p.Service.Equals("MsgService", StringComparison.OrdinalIgnoreCase));
                ;
                if (!services.Any())
                {
                    Console.WriteLine("服务器找不到实例对象");
                }
                else
                {
                  var service=  services.ElementAt(Environment.TickCount %services.Count());
                   Console.WriteLine($"id={service.ID},服务名字={service.Service},{service},Ip={service.Address},port={service.Port}");
                }
                //foreach (var item in services.Values)
                //{
                //    Console.WriteLine($"id={item.ID},服务名字={item.Service},{item},Ip={item.Address},port={item.Port}");
                //}
            }
            Console.ReadKey();
        }
    }
}
