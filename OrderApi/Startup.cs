using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using OrderApi.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.OpenApi.Models;
using OrderApi.Filter;
using System.Net.Mime;

namespace OrderApi
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
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddDbContext<OrderContext>(opt =>
            // opt.UseInMemoryDatabase("OrderDB"));
             opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddHostedService<EventReceiverService>();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            c.SwaggerDoc("v1", new OpenApiInfo {
                Version = "v1",
                Title = "ToDo API",
                Description = "A simple example ASP.NET Core Web API",
                TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Shayne Boyer",
                    Email = string.Empty,
                    Url = new Uri("https://twitter.com/spboyer"),
                },
                License = new OpenApiLicense
                {
                    Name = "Use under LICX",
                    Url = new Uri("https://example.com/license"),
                }
            }));

            services.AddControllers(options=> options.Filters.Add(new HttpResponseExceptionFilter()))
                .ConfigureApiBehaviorOptions(options=>
                {
                    options.InvalidModelStateResponseFactory = context => {
                        var result = new BadRequestObjectResult(context.ModelState);
                        // TODO: add `using using System.Net.Mime;` to resolve MediaTypeNames
                        result.ContentTypes.Add(MediaTypeNames.Application.Json);
                        result.ContentTypes.Add(MediaTypeNames.Application.Xml);
                        return result;
                    }; 
                })
             .AddNewtonsoftJson(options => options.UseMemberCasing());
        }





        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("MyPolicy");
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/error-local-development");
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
