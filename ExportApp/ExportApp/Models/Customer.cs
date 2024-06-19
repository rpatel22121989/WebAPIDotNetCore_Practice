using System;
using System.Collections.Generic;

namespace ExportApp.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string DateOfBirth { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string MaritalStatus { get; set; } = null!;

    public string CurrentAddress { get; set; } = null!;

    public string PermanentAddress { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Nationality { get; set; } = null!;

    public string Pincode { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
