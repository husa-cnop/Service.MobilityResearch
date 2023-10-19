using FreEPPlus;

namespace MobilityResearch.PartUsage;

internal class PartUsageRow
{
    [ExcelColumn("RowVer")]
    public string? RowVer { get; set; }

    [ExcelColumn("Region")]
    public string? Region { get; set; }

    [ExcelColumn("Zone")]
    public string? Zone { get; set; }

    [ExcelColumn("Tech")]
    public string? Tech { get; set; }

    [ExcelColumn("UserName")]
    public string? UserName { get; set; }

    [ExcelColumn("Modified", "m/d/yy h:mm AM/PM;@")]
    public string? Modified { get; set; }

    [ExcelColumn("ReferenceModule")]
    public string? ReferenceModule { get; set; }

    [ExcelColumn("ReferenceId")]
    public string? ReferenceId { get; set; }

    [ExcelColumn("StorageLocation")]
    public string? StorageLocation { get; set; }

    [ExcelColumn("ItemId")]
    public string? ItemId { get; set; }

    [ExcelColumn("DispositionReason")]
    public string? DispositionReason { get; set; }

    [ExcelColumn("InQty")]
    public int? InQty { get; set; }

    [ExcelColumn("OutQty")]
    public int? OutQty { get; set; }

    [ExcelColumn("InventoryQty")]
    public int? InventoryQty { get; set; }
}