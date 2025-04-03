using System;
using System.Collections.Generic;

namespace PR18;

public partial class OptSale
{
    public int Id { get; set; }

    public string NameProduct { get; set; } = null!;

    public decimal PriceProduct { get; set; }

    public DateTime? DateOfReceipt { get; set; }

    public int NumberBatch { get; set; }

    public string SizeBatch { get; set; } = null!;

    public string NameCompany { get; set; } = null!;

    public string? SizeSellBatch { get; set; }

    public DateTime? DateSellBatch { get; set; }

    public decimal PriceBatch { get; set; }
}
