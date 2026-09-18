namespace CarePulseERP.Models;

public enum StaffRole
{
    Doctor = 1,
    HeadNurse = 2,
    DutyNurse = 3,
    Receptionist = 4,
    Pharmacist = 5,
    SweeperJanitor = 6,
    LabTechnician = 7,
    HospitalAdmin = 8
}

public enum UrgencyLevel
{
    Routine = 1,
    Priority = 2,
    Urgent = 3,
    Critical = 4
}

public enum PayrollStatus
{
    Pending = 1,
    Processing = 2,
    Paid = 3,
    OnHold = 4
}

public enum ShiftType
{
    Morning = 1,
    Evening = 2,
    Night = 3,
    Rotational = 4
}

public enum BedStatus
{
    Vacant = 1,
    Occupied = 2,
    Maintenance = 3,
    Reserved = 4
}
