using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarePulseERP.Data;
using CarePulseERP.DTOs;
using CarePulseERP.Models;

namespace CarePulseERP.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PayrollController : ControllerBase
{
    private readonly HospitalDbContext _db;

    public PayrollController(HospitalDbContext db)
    {
        _db = db;
    }

    // GET: api/payroll/summary
    [HttpGet("summary")]
    public async Task<IActionResult> GetPayrollSummary([FromQuery] string? monthYear = null)
    {
        var targetMonth = string.IsNullOrWhiteSpace(monthYear) 
            ? DateTime.UtcNow.ToString("MMMM yyyy") 
            : monthYear;

        var records = await _db.SalaryRecords
            .Include(r => r.StaffMember)
            .Where(r => r.MonthYear == targetMonth)
            .ToListAsync();

        var totalPayroll = records.Sum(r => r.NetSalary);
        var totalDisbursed = records.Where(r => r.Status == PayrollStatus.Paid).Sum(r => r.NetSalary);
        var totalPending = records.Where(r => r.Status == PayrollStatus.Pending).Sum(r => r.NetSalary);

        var summary = new PayrollSummaryDto
        {
            MonthYear = targetMonth,
            TotalStaffCount = records.Count,
            TotalPayrollAmount = totalPayroll,
            TotalDisbursed = totalDisbursed,
            TotalPending = totalPending,
            PaidCount = records.Count(r => r.Status == PayrollStatus.Paid),
            PendingCount = records.Count(r => r.Status == PayrollStatus.Pending)
        };

        return Ok(summary);
    }

    // GET: api/payroll/records
    [HttpGet("records")]
    public async Task<IActionResult> GetSalaryRecords([FromQuery] string? monthYear = null, [FromQuery] PayrollStatus? status = null)
    {
        var targetMonth = string.IsNullOrWhiteSpace(monthYear) 
            ? DateTime.UtcNow.ToString("MMMM yyyy") 
            : monthYear;

        var query = _db.SalaryRecords
            .Include(r => r.StaffMember)
            .Where(r => r.MonthYear == targetMonth)
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(r => r.Status == status.Value);
        }

        var records = await query.ToListAsync();
        return Ok(records);
    }

    // POST: api/payroll/disburse
    [HttpPost("disburse")]
    public async Task<IActionResult> DisburseSalary([FromBody] DisburseSalaryDto dto)
    {
        var record = await _db.SalaryRecords
            .Include(r => r.StaffMember)
            .FirstOrDefaultAsync(r => r.StaffMemberId == dto.StaffMemberId && r.MonthYear == dto.MonthYear);

        if (record == null)
        {
            var staff = await _db.StaffMembers.FindAsync(dto.StaffMemberId);
            if (staff == null) return NotFound(new { message = "Staff member not found." });

            record = new SalaryRecord
            {
                StaffMemberId = staff.Id,
                MonthYear = dto.MonthYear,
                BaseSalary = staff.MonthlySalary
            };
            await _db.SalaryRecords.AddAsync(record);
        }

        record.OvertimeAllowance = dto.OvertimeAllowance;
        record.Deductions = dto.Deductions;
        record.Status = PayrollStatus.Paid;
        record.PaidDate = DateTime.UtcNow;
        record.TransactionReference = $"TX-PAY-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}";
        record.Remarks = dto.Remarks;

        await _db.SaveChangesAsync();

        return Ok(new 
        { 
            message = "Salary disbursed successfully.", 
            staffName = record.StaffMember?.FullName,
            netSalary = record.NetSalary,
            txRef = record.TransactionReference,
            paidDate = record.PaidDate
        });
    }
}
