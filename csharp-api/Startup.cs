﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaFpCSharpTodoBackEndTotallyNotButtCheeks.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MegaFpCSharpTodoBackEndTotallyNotButtCheeks
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
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services
                .AddCors();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services
                .AddDbContextPool<TodoDbContext>(
                    options => options.UseSqlServer(
                        Configuration
                            .GetConnectionString("TodoDatabase")
                    )
                );

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app
                    .UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app
                    .UseHsts();
            }

            app
                .UseCors(
                    builder => builder
                        .WithOrigins("http://localhost:8000")
                );

            app
                .UseHttpsRedirection();

            app
                .UseMvc();
        }
    }
}
