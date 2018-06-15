using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SpyStore.DAL.EF;
using SpyStore.DAL.Initializers;
using SpyStore.DAL.Repos;
using SpyStore.DAL.Repos.Interfaces;
using SpyStore.Service.Filters;

namespace SpyStore.Service
{
    public class Startup
    {
        private IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("app.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            _env = env;
            //Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore(config => config.Filters.Add(new SpyStoreExceptionFilter(_env.IsDevelopment())))
                .AddJsonFormatters(j =>
            {
                j.ContractResolver = new DefaultContractResolver();
                j.Formatting = Formatting.Indented;
            });


            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => { builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials(); });
            });
            var connString =
                $"Host = {Configuration["pg_host"]}; Port =  {Configuration["pg_port"]}; Database =  {Configuration["pg_db"]}; Username =  {Configuration["pg_user"]}; Password =  {Configuration["pg_pass"]}";

            services.AddDbContext<StoreContext>(options => options.UseNpgsql(connString));

          

            services.AddScoped<ICategoryRepo, CategoryRepo>();
            services.AddScoped<IProductRepo, ProductRepo>();
            services.AddScoped<ICustomerRepo, CustomerRepo>();
            services.AddScoped<IShoppingCartRepo, ShoppingCartRepo>();
            services.AddScoped<IOrderRepo, OrderRepo>();
            services.AddScoped<IOrderDetailRepo, OrderDetailRepo>();

            //services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // _env = env;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                loggerFactory
                    .AddConsole(Configuration.GetSection("Logging"))
                    .AddDebug();

                StoreDataInitializer.InitializeData(app.ApplicationServices);

            }

            app.UseCors("AllowAll");
            

            app.UseMvc();
        }
    }
}