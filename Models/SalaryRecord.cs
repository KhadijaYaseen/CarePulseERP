using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarePulseERP.Models;

[Table("SalaryRecords")]
public class SalaryRecord
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int StaffMemberId { get; set; }

    [ForeignKey(nameof(StaffMemberId))]
    public virtual StaffMember? StaffMember { get; set; }

    [Required]
    [MaxLength(20)]
    public string MonthYear { get; set; } = string.Empty; // e.g. "August 2026"

    [Column(TypeName = "decimal(18,2)")]
    public decimal BaseSalary { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal OvertimeAllowance { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Deductions { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal NetSalary => BaseSalary + OvertimeAllowance - Deductions;

    public PayrollStatus Status { get; set; } = PayrollStatus.Pending;

    public DateTime? PaidDate { get; set; }

    [MaxLength(60)]
    public string TransactionReference { get; set; } = string.Empty;

    public string? Remarks { get; set; }
}
