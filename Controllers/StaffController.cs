using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarePulseERP.Data;
using CarePulseERP.DTOs;
using CarePulseERP.Models;

namespace CarePulseERP.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StaffController : ControllerBase
{
    private readonly HospitalDbContext _db;

    public StaffController(HospitalDbContext db)
    {
        _db = db;
    }

    // GET: api/staff
    [HttpGet]
    public async Task<IActionResult> GetAllStaff([FromQuery] StaffRole? role = null, [FromQuery] string? search = null)
    {
        var query = _db.StaffMembers.AsNoTracking().AsQueryable();

        if (role.HasValue)
        {
            query = query.Where(s => s.Role == role.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(s => s.FullName.Contains(search) || s.Department.Contains(search));
        }

        var staff = await query.OrderByDescending(s => s.DateJoined).ToListAsync();
        return Ok(staff);
    }

    // GET: api/staff/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStaffById(int id)
    {
        var staff = await _db.StaffMembers
            .Include(s => s.SalaryRecords)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (staff == null) return NotFound(new { message = $"Staff member with ID {id} not found." });
        return Ok(staff);
    }

    // POST: api/staff (Create Staff / Doctor / Nurse / Sweeper)
    [HttpPost]
    public async Task<IActionResult> CreateStaff([FromBody] CreateStaffDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var staff = new StaffMember
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Role = dto.Role,
            Department = dto.Department,
            MonthlySalary = dto.MonthlySalary,
            Shift = dto.Shift,
            IsActive = true,
            DateJoined = DateTime.UtcNow
        };

        await _db.StaffMembers.AddAsync(staff);
        await _db.SaveChangesAsync();

        // Auto-create initial pending salary record for current month
        var currentMonth = DateTime.UtcNow.ToString("MMMM yyyy");
        var initialSalary = new SalaryRecord
        {
            StaffMemberId = staff.Id,
            MonthYear = currentMonth,
            BaseSalary = staff.MonthlySalary,
            Status = PayrollStatus.Pending
        };

        await _db.SalaryRecords.AddAsync(initialSalary);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetStaffById), new { id = staff.Id }, staff);
    }

    // PUT: api/staff/{id} (Edit / Update Staff Details & Salary)
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStaff(int id, [FromBody] CreateStaffDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var staff = await _db.StaffMembers.FindAsync(id);
        if (staff == null) return NotFound(new { message = $"Staff member with ID {id} not found." });

        staff.FullName = dto.FullName;
        staff.Email = dto.Email;
        staff.PhoneNumber = dto.PhoneNumber;
        staff.Role = dto.Role;
        staff.Department = dto.Department;
        staff.MonthlySalary = dto.MonthlySalary;
        staff.Shift = dto.Shift;

        // Update current month pending salary record if base salary changed
        var currentMonth = DateTime.UtcNow.ToString("MMMM yyyy");
        var pendingSalary = await _db.SalaryRecords
            .FirstOrDefaultAsync(r => r.StaffMemberId == id && r.MonthYear == currentMonth && r.Status == PayrollStatus.Pending);

        if (pendingSalary != null)
        {
            pendingSalary.BaseSalary = dto.MonthlySalary;
        }

        await _db.SaveChangesAsync();
        return Ok(new { message = "Staff member updated successfully.", staff });
    }

    // DELETE: api/staff/{id} (Delete / Terminate Staff Member)
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStaff(int id)
    {
        var staff = await _db.StaffMembers.FindAsync(id);
        if (staff == null) return NotFound(new { message = $"Staff member with ID {id} not found." });

        _db.StaffMembers.Remove(staff);
        await _db.SaveChangesAsync();

        return Ok(new { message = $"Staff member '{staff.FullName}' removed successfully.", id });
    }

    // PUT: api/staff/{id}/toggle-status
    [HttpPut("{id}/toggle-status")]
    public async Task<IActionResult> ToggleStaffStatus(int id)
    {
        var staff = await _db.StaffMembers.FindAsync(id);
        if (staff == null) return NotFound();

        staff.IsActive = !staff.IsActive;
        await _db.SaveChangesAsync();

        return Ok(new { id = staff.Id, isActive = staff.IsActive, message = "Status updated successfully." });
    }
}
