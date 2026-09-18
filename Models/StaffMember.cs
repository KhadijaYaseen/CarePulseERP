using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarePulseERP.Models;

[Table("StaffMembers")]
public class StaffMember
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(120)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(120)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(25)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    public StaffRole Role { get; set; }

    [Required]
    [MaxLength(80)]
    public string Department { get; set; } = "General Operations";

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal MonthlySalary { get; set; }

    public ShiftType Shift { get; set; } = ShiftType.Morning;

    public bool IsActive { get; set; } = true;

    public DateTime DateJoined { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ICollection<SalaryRecord> SalaryRecords { get; set; } = new List<SalaryRecord>();
    public virtual ICollection<Patient> AssignedPatients { get; set; } = new List<Patient>();
}
