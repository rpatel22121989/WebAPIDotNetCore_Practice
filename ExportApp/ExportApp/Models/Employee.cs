using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExportApp.Models;

[Table("Employee")]
public partial class Employee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EmployeeId { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string DateOfBirth { get; set; } = null!;

    [Required]
    public string Gender { get; set; } = null!;

    [Required]
    public string MaritalStatus { get; set; } = null!;

    [Required]
    public string CurrentAddress { get; set; } = null!;

    public string? PermanentAddress { get; set; }

    [Required]
    public string City { get; set; } = null!;

    [Required]
    public string Nationality { get; set; } = null!;

    [Required]
    public string Pincode { get; set; } = null!;

    public int? Age { get; set; }

    public string? State { get; set; }

    public decimal? Salary { get; set; }

    public string? Password { get; set; }
}
