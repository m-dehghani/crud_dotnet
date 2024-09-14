using Asp.Versioning;
using Confluent.Kafka;
using Mc2.CrudTest.Presentation.Server.DomainServices;
using Mc2.CrudTest.Presentation.Server.Infrastructure;
using Mc2.CrudTest.Presentation.Shared.Helper;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Mc2.CrudTest.Presentation.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            
           // builder.AddServiceDefaults();

            builder.Services.AddControllersWithViews().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
            }); 
            
           // builder.AddNpgsqlDataSource("EventStoreConnection");
            
            builder.Services.AddRazorPages();
            
            builder.AddNpgsqlDbContext<ApplicationDbContext>("EventStoreConnection");
            
            builder.AddNpgsqlDbContext<ReadModelDbContext>("EventStoreConnection");


            builder.Services.AddSwaggerGen();
            
            builder.Services.
                AddMediatR(cfg =>
                {
                    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
                });
            
            builder.Services.AddScoped<IDatabase>(cfg =>
            {
                IConnectionMultiplexer multiplexer = ConnectionMultiplexer.Connect($"{builder.Configuration["RedisUrl"]}");
            
                return multiplexer.GetDatabase();
            });

            // builder.Services.AddTransient<ICustomerService, CustomerService>();
            
            builder.Services.AddTransient<ICustomerService, SlowerCustomerService>();
            
            builder.Services.AddTransient<IEventRepository, EventStoreRepository>();
          
            builder.Services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
           
                options.DefaultApiVersion = new ApiVersion(1, 0);
           
                options.AssumeDefaultVersionWhenUnspecified = true;
            });
            
            // builder.AddKafkaProducer<string, string>("kafka");
            //
            // builder.AddKafkaConsumer<string, string>("kafka");

            WebApplication app = builder.Build();

            // run the required migration
            using (IServiceScope scope = app.Services.CreateScope())
            {
                // IProducer<string, string> producer = scope.ServiceProvider.GetRequiredService<IProducer<string, string>>();
                // Message<string, string> msg = new Message<string, string>()
                // {
                //     Key = "testKey",
                //     Value = "testValue",
                // };
                //
                // producer.Produce(new TopicPartition("test",Partition.Any),msg);
                
                ApplicationDbContext db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    db.Database.Migrate();
               
                // ReadModelDbContext readDb = scope.ServiceProvider.GetRequiredService<ReadModelDbContext>();
                //     readDb.Database.Migrate();
            }
            
            // app.MapDefaultEndpoints();

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