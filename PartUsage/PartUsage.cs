using System.Text;
using Cackle.ConsoleApp;
using CommunityToolkit.Diagnostics;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using Service.CommonTypes;
using ServiceMobilityRepository;

namespace MobilityResearch.PartUsage;

internal class PartUsage(ServiceMobility mob) : ICommandAsync<PartUsageArgs>
{
    /// <summary>
    /// </summary>
    /// <param name="args"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<int> InvokeAsync(PartUsageArgs args, CancellationToken ct)
    {
        var basePath = @"C:\Temp";
        var fileName = $"{basePath}\\PartUsage-{args.UserName}-{args.StartDate:yyyy-MM-dd}.xlsx";
        var file = new FileInfo(fileName);

        if (file.Exists) file.Delete();

        var records = await GetRecords(args.StartDate, args.EndDate, args.UserName, ct);
        CreateSheet(file, records);

        return 1;
    }

    /// <summary>
    /// Create output report
    /// </summary>
    /// <param name="outputFile">Output File</param>
    /// <param name="export">Records</param>
    private static void CreateSheet(FileInfo outputFile, List<PartUsageRow> export)
    {
        using var excel = new ExcelPackage(outputFile);

        var dataSheet = excel.Workbook.Worksheets.Add("Export");
        dataSheet.Cells["A1"].LoadFromCollection(export, true, TableStyles.Medium2);

        // Let's adjust column widths
        var dataTable = dataSheet.Tables.FirstOrDefault();
        Guard.IsNotNull(dataTable, nameof(dataTable));
        dataSheet.Cells[dataTable.Address.Address].AutoFitColumns();
        dataTable.Name = "Export";

        excel.Save();
    }

    /// <summary>
    ///     Gather records from ServiceMobility
    /// </summary>
    /// <param name="startDate">Start Date</param>
    /// <param name="endDate">End Date</param>
    /// <param name="userName">UserName</param>
    /// <param name="ct">See <see cref="CancellationToken" /></param>
    public async Task<List<PartUsageRow>> GetRecords(DateTimeOffset startDate, DateTimeOffset endDate, string userName,
        CancellationToken ct)
    {
        var records = await mob.Users.InventoryUsage(startDate, endDate, userName, ct);

        var rows = new List<PartUsageRow>();
        foreach (var record in records)
        {
            var usage = record.InventoryUsage;
            var rzt = new RegionZoneTech(usage.UserID);

            foreach (var location in record.InventoryUsage.StorageLocations)
            foreach (var item in location.Items)
            {
                rows.Add(new PartUsageRow
                {
                    RowVer = record.rowver,
                    Region = rzt.Region,
                    Zone = rzt.Zone,
                    Tech = rzt.Tech,
                    UserName = usage.UserID,
                    Modified = usage.ModifiedUTC.LocalDateTime.ToString("G"),
                    ReferenceModule = usage.RefModule.ToString("G"),
                    ReferenceId = usage.RefObjID,
                    StorageLocation = location.StorageLocationID,
                    ItemId = item.ProdNum,
                    DispositionReason = string.Empty,
                    InQty = item.InQty,
                    OutQty = item.OutQty,
                    InventoryQty = item.InventoryQty
                });

                if (item.DispositionReasons is null) continue;
                foreach (var reason in item.DispositionReasons)
                {
                    rows.Add(new PartUsageRow
                    {
                        RowVer = record.rowver,
                        Region = rzt.Region,
                        Zone = rzt.Zone,
                        Tech = rzt.Tech,
                        UserName = usage.UserID,
                        Modified = usage.ModifiedUTC.LocalDateTime.ToString("G"),
                        ReferenceModule = usage.RefModule.ToString("G"),
                        ReferenceId = usage.RefObjID,
                        StorageLocation = location.StorageLocationID,
                        ItemId = item.ProdNum,
                        DispositionReason = reason.ReasonCode.ToString("G"),
                        InQty = reason.InQty,
                        OutQty = reason.OutQty,
                        InventoryQty = reason.InventoryQty
                    });
                }
            }
        }

        return rows;
    }
}