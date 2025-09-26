using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using System.IO;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration((hostContext, config) =>
    {
        // Print out current directory and base directory
        Console.WriteLine($"Current Directory: {Directory.GetCurrentDirectory()}");
        Console.WriteLine($"Base Directory: {AppContext.BaseDirectory}");

        var basePath = AppContext.BaseDirectory;
        var appsettingsPath = Path.Combine(basePath, "appsettings.json");

        // Check if file exists
        Console.WriteLine($"appsettings.json exists: {File.Exists(appsettingsPath)}");
        Console.WriteLine($"appsettings.json path: {appsettingsPath}");

        config.SetBasePath(basePath)
              .AddJsonFile(appsettingsPath, optional: true, reloadOnChange: true);
    })
    .Build();

host.Run();
