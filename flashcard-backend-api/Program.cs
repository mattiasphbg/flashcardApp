using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore; 
using flashcard_backend_api.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Configuration; 
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Http.AspNetCore; 

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication() 
    .ConfigureAppConfiguration((hostContext, config) =>
    {
    
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        config.AddEnvironmentVariables(); 
    })
    .ConfigureServices((hostContext, services) =>
    {
        
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

  
        var configuration = hostContext.Configuration;
        var connectionString = configuration.GetConnectionString("SqlDbConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("SqlDbConnection connection string is not set in appsettings.json or environment variables.");
        }

     
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

      
        services.AddSingleton(new Random());


        services.AddControllers().AddNewtonsoftJson();
    })
    .Build();

host.Run();
