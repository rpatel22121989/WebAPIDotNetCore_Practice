using System;
using System.Collections.Generic;

namespace ExportApp.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal ProductPrice { get; set; }

    public int Quantity { get; set; }

    public int CustomerId { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
