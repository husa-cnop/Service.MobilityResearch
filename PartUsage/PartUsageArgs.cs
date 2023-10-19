using Cackle.ConsoleApp;
using CommandLine;
using Service.CommonTypes;

namespace MobilityResearch.PartUsage;

[Verb("usage", true)]
internal class PartUsageArgs : ICommandArgs
{
    [Option('u', "users", Required = true,
        HelpText = "ServiceMobility user name")]
    public RegionZoneTech UserName { get; set; }

    [Option('s', "startDate", Required = false, 
        HelpText = "Start date of report; defaults to 24 hours ago")]
    public DateTimeOffset StartDate { get; set; } = DateTimeOffset.Now.AddDays(-1);

    [Option('e', "endDate", Required = false, 
        HelpText = "End date of report; defaults to now")]
    public DateTimeOffset EndDate { get; set; } = DateTimeOffset.Now;
}