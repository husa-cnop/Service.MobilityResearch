using Cackle.ConsoleApp;
using MobilityResearch.PartUsage;
using ServiceMobilityRepository;

var builder = CommandHostBuilder.Create();
builder.RegisterServices((services, config) =>
{
    services.AddServiceMobility(config.GetSection("ServiceMobility"));
});

builder.RegisterAsyncCommand<PartUsage, PartUsageArgs>();

return await builder.Build().RunAsync(args);