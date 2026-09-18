using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarePulseERP.Models;

[Table("Patients")]
public class Patient
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(120)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(30)]
    public string AgeGender { get; set; } = string.Empty;

    [MaxLength(25)]
    public string ContactNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(80)]
    public string Department { get; set; } = "Emergency";

    public UrgencyLevel Urgency { get; set; } = UrgencyLevel.Routine;

    [MaxLength(80)]
    public string Status { get; set; } = "Triaged"; // Triaged, In Consult, Admitted, Discharged

    public string? MedicalNotes { get; set; }

    public int? AssignedDoctorId { get; set; }
    [ForeignKey(nameof(AssignedDoctorId))]
    public virtual StaffMember? AssignedDoctor { get; set; }

    public int? AssignedNurseId { get; set; }
    [ForeignKey(nameof(AssignedNurseId))]
    public virtual StaffMember? AssignedNurse { get; set; }

    public int? BedId { get; set; }

    public DateTime AdmittedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DischargedAt { get; set; }
}

[Table("Beds")]
public class Bed
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(40)]
    public string BedCode { get; set; } = string.Empty; // e.g. "ICU-A-04"

    [Required]
    [MaxLength(60)]
    public string WardName { get; set; } = string.Empty; // e.g. "Intensive Care Unit"

    public BedStatus Status { get; set; } = BedStatus.Vacant;

    public int? CurrentPatientId { get; set; }

    public decimal DailyRate { get; set; } = 150.00m;
}
