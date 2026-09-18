using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarePulseERP.Data;
using CarePulseERP.DTOs;
using CarePulseERP.Models;

namespace CarePulseERP.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly HospitalDbContext _db;

    public PatientsController(HospitalDbContext db)
    {
        _db = db;
    }

    // GET: api/patients
    [HttpGet]
    public async Task<IActionResult> GetPatients([FromQuery] string? status = null, [FromQuery] string? department = null)
    {
        var query = _db.Patients
            .Include(p => p.AssignedDoctor)
            .Include(p => p.AssignedNurse)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(p => p.Status == status);

        if (!string.IsNullOrWhiteSpace(department))
            query = query.Where(p => p.Department == department);

        var patients = await query.OrderByDescending(p => p.AdmittedAt).ToListAsync();
        return Ok(patients);
    }

    // POST: api/patients/admit
    [HttpPost("admit")]
    public async Task<IActionResult> AdmitPatient([FromBody] AdmitPatientDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // Auto-assign available doctor if not explicitly supplied
        int? doctorId = dto.AssignedDoctorId;
        if (!doctorId.HasValue)
        {
            var onDutyDoctor = await _db.StaffMembers
                .Where(s => s.Role == StaffRole.Doctor && s.Department == dto.Department && s.IsActive)
                .Select(s => (int?)s.Id)
                .FirstOrDefaultAsync();

            doctorId = onDutyDoctor;
        }

        var patient = new Patient
        {
            FullName = dto.FullName,
            AgeGender = dto.AgeGender,
            ContactNumber = dto.ContactNumber,
            Department = dto.Department,
            Urgency = dto.Urgency,
            Status = "Triaged",
            MedicalNotes = dto.MedicalNotes,
            AssignedDoctorId = doctorId,
            AdmittedAt = DateTime.UtcNow
        };

        await _db.Patients.AddAsync(patient);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPatients), new { id = patient.Id }, patient);
    }

    // PUT: api/patients/{id}/update-status
    [HttpPut("{id}/update-status")]
    public async Task<IActionResult> UpdatePatientStatus(int id, [FromBody] string newStatus)
    {
        var patient = await _db.Patients.FindAsync(id);
        if (patient == null) return NotFound();

        patient.Status = newStatus;
        if (newStatus == "Discharged")
        {
            patient.DischargedAt = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync();
        return Ok(patient);
    }
}
