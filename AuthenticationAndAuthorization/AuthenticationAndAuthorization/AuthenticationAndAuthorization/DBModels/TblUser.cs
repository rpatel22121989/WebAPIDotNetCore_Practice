using System;
using System.Collections.Generic;

namespace AuthenticationAndAuthorization.DBModels;

public partial class TblUser
{
    public decimal Id { get; set; }

    public string? FullName { get; set; }

    public string EmailId { get; set; } = null!;

    public string? Password { get; set; }

    public string? Designation { get; set; }

    public DateTime? CreatedDate { get; set; }
}
