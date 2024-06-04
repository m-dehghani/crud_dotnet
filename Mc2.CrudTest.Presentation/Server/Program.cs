using System.Configuration;
using Asp.Versioning;
using FluentAssertions.Common;
using Mc2.CrudTest.Presentation.DomainServices;
using Mc2.CrudTest.Presentation.Infrastructure;
using Mc2.CrudTest.Presentation.Server.Controllers;
using MediatR;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace Mc2.CrudTest.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddServiceDefaults();

            
            // Add services to the container.

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration["ConnectionStrings:EventStoreConnection"]));
            builder.Services.AddDbContext<ReadModelDbContext>(options =>
                options.UseNpgsql(builder.Configuration["ConnectionStrings:EventStoreConnection"]));
            builder.Services.AddSwaggerGen();
            builder.Services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
            });
            builder.Services.AddScoped<IDatabase>(cfg =>
            {
                IConnectionMultiplexer multiplexer = ConnectionMultiplexer.Connect($"{builder.Configuration["RedisUrl"]}");
                return multiplexer.GetDatabase();
            });
            
            builder.Services.AddTransient<ICustomerService, CustomerService>();
            builder.Services.AddTransient<IEventRepository, EventStoreRepository>();
            builder.Services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.Migrate();
            }
            app.MapDefaultEndpoints();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();


            app.MapRazorPages();
            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}