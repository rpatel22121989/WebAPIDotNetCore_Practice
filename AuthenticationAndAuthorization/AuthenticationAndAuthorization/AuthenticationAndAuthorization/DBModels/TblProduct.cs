using System;
using System.Collections.Generic;

namespace AuthenticationAndAuthorization.DBModels;

public partial class TblProduct
{
    public decimal Id { get; set; }

    public string? ProductCode { get; set; }

    public string? ProductName { get; set; }

    public decimal? Categoryid { get; set; }

    public double? Price { get; set; }

    public double? StkQty { get; set; }

    public decimal? VendorId { get; set; }

    public virtual TblCategory? Category { get; set; }

    public virtual TblVendor? Vendor { get; set; }
}
