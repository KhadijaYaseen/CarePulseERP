using System.ComponentModel.DataAnnotations;
using CarePulseERP.Models;

namespace CarePulseERP.DTOs;

public class CreateStaffDto
{
    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    public StaffRole Role { get; set; }

    [Required]
    public string Department { get; set; } = string.Empty;

    [Required]
    [Range(100, 1000000)]
    public decimal MonthlySalary { get; set; }

    public ShiftType Shift { get; set; } = ShiftType.Morning;
}

public class DisburseSalaryDto
{
    [Required]
    public int StaffMemberId { get; set; }

    [Required]
    public string MonthYear { get; set; } = string.Empty;

    public decimal OvertimeAllowance { get; set; } = 0;
    public decimal Deductions { get; set; } = 0;
    public string? Remarks { get; set; }
}

public class AdmitPatientDto
{
    [Required]
    public string FullName { get; set; } = string.Empty;

    public string AgeGender { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;

    [Required]
    public string Department { get; set; } = "Emergency";

    public UrgencyLevel Urgency { get; set; } = UrgencyLevel.Routine;
    public string? MedicalNotes { get; set; }
    public int? AssignedDoctorId { get; set; }
}

public class PayrollSummaryDto
{
    public string MonthYear { get; set; } = string.Empty;
    public int TotalStaffCount { get; set; }
    public decimal TotalPayrollAmount { get; set; }
    public decimal TotalDisbursed { get; set; }
    public decimal TotalPending { get; set; }
    public int PaidCount { get; set; }
    public int PendingCount { get; set; }
}
