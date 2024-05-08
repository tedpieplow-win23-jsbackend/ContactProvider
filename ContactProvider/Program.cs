using ContactProvider.Data;
using ContactProvider.Factories;
using ContactProvider.Repositories;
using ContactProvider.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        services.AddScoped<ContactService>();
        services.AddScoped<ContactFactory>();
        services.AddScoped<ContactRepository>();

        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddDbContext<DataContext>(x => x.UseSqlServer(context.Configuration.GetConnectionString("Database")));
    })
    .Build();

host.Run();
