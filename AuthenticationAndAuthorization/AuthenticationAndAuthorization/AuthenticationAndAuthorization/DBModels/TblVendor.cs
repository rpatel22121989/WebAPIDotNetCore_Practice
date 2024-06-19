using System;
using System.Collections.Generic;

namespace AuthenticationAndAuthorization.DBModels;

public partial class TblVendor
{
    public decimal Id { get; set; }

    public string? VendorCode { get; set; }

    public string? VendorName { get; set; }

    public string? VendorPhone { get; set; }

    public string? VendorEmail { get; set; }

    public virtual ICollection<TblProduct> TblProducts { get; } = new List<TblProduct>();
}
