using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using CoreBackend.Api.Service;
using Microsoft.Extensions.Configuration;
using CoreBackend.Api.Entities;
using Microsoft.EntityFrameworkCore;
using CoreBackend.Api.Repositories;
using CoreBackend.Api.Dtos;

namespace CoreBackend.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public static IConfiguration Configuration { get; private set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            //ConfigureServices方法是用来把services(各种服务, 例如identity, ef, mvc等等包括第三方的, 或者自己写的)加入(register)到container(asp.net core的容器)中去, 并配置这些services
            //这个container是用来进行dependency injection的(依赖注入). 所有注入的services(此外还包括一些框架已经注册好的services) 在以后写代码的时候,
            //都可以将它们注入(inject)进去. 例如上面的Configure方法的参数, app, env, loggerFactory都是注入进去的services.
            services.AddMvc();
#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif
            var connectionString = Configuration["connectionStrings:productionInfoDbConnectionString"];
            services.AddDbContext<MyContext>(o => o.UseSqlServer(connectionString));
            services.AddScoped<IProductRepository, ProductRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();
            //Configure方法是asp.net core程序用来具体指定如何处理每个http请求的, 例如我们可以让这个程序知道我使用mvc来处理http请求, 那就调用app.UseMvc()这个方法就行
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }
            app.UseStatusCodePages();
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Product, ProductWithoutMaterialDto>();
                cfg.CreateMap<Product, ProductDto>();
                cfg.CreateMap<Material, MaterialDto>();
                cfg.CreateMap<ProductCreation,Product>();
                cfg.CreateMap<ProductModification, Product>();
            });
            app.UseMvc();
            //应该在处理异常的middleware后边调用app.UserMvc()，处理异常的middleware可以在把request交给MVC之前就处理异常，更重要的是它还可以捕获并处理返回MVC相关代码执行中的异常。

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
