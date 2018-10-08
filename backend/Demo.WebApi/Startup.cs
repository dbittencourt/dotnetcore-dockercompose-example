using Demo.Data;
using Demo.Data.Repositories;
using Demo.Data.Services;
using Demo.Shared.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Net.Http;

namespace Demo
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
            // sets up custom settings
            services.AddOptions();
            services.AddSingleton(Configuration);

            services.AddDbContext<DemoDbContext>(options =>
            {
                options.UseSqlite("Data Source=Demo.db");
            });

            // HttpClient is recommend to be used as a singleton
            services.AddSingleton(new HttpClient());
            services.AddSingleton<IHttpClient, ApiClient>();

            services.AddScoped<ICache, RedisCache>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IPetRepository, PetRepository>();

            services.AddTransient<IPeopleService, PeopleService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Demo Person API", Version = "v1" });
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "Demo.WebApi.xml");
                c.IncludeXmlComments(xmlPath);
            });

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Analyser API V1"));

            // Enable cross origin requests
            app.UseCors(options =>
            {
                options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials();
            });

            app.UseMvc();
        }
    }
}
