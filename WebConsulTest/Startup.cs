using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebConsulTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            app.UseMvc();
            string ip = Configuration["ip"];
            int port = Convert.ToInt32(Configuration["port"]);
            string serviceName = "MsgService";
            string serviceId = serviceName + Guid.NewGuid();
            using (var client = new ConsulClient(ConsulConfig))
            {
                //注册服务到 consul
                client.Agent.ServiceRegister(new AgentServiceRegistration()
                {
                    ID = serviceId,//服务编号，不能重复，用 Guid 最简单 
                    Name = serviceName,//服务名
                    Address = ip,//服务提供者的能被消费者访问的 ip 地址
                    Port = port,//服务提供者的能被消费者访问的端口 
                    Check = new AgentServiceCheck
                    {
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务停止多久后反注销
                        Interval = TimeSpan.FromSeconds(10),//健康检查的时间间隔，心跳间隔。
                        HTTP = $"http://{ip}:{port}/api/health",//健康检查的地址
                        Timeout = TimeSpan.FromSeconds(5)
                    }
                }).Wait();//consul客户端所有方法基本都是异步方法，但是都没按照规范加上Async后缀，所以容易误导，记得调用后加Wait(),或者 await
            }
            applicationLifetime.ApplicationStopped.Register(() =>
            {
                using (var client = new ConsulClient(ConsulConfig))
                {
                    client.Agent.ServiceDeregister(serviceId).Wait();
                }
            });
        }
        private void ConsulConfig(ConsulClientConfiguration c)
        {
            c.Address = new Uri("http://127.0.0.1:8500");
            c.Datacenter = "dc1";
        }

    }
}
