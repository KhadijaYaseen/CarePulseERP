-- ==========================================================
-- CarePulse MediCore — Enterprise Hospital Management ERP
-- Microsoft SQL Server (T-SQL) Production Schema
-- Designed & Architected by Khadija Yaseen
-- ==========================================================

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'CarePulseHospitalDB')
BEGIN
    CREATE DATABASE CarePulseHospitalDB;
END
GO

USE CarePulseHospitalDB;
GO

-- 1. Table: StaffMembers
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='StaffMembers' AND xtype='U')
BEGIN
    CREATE TABLE StaffMembers (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        FullName NVARCHAR(120) NOT NULL,
        Email NVARCHAR(120) NOT NULL UNIQUE,
        PhoneNumber NVARCHAR(25) NULL,
        Role INT NOT NULL, -- 1=Doctor, 2=HeadNurse, 3=DutyNurse, 4=Receptionist, 5=Pharmacist, 6=SweeperJanitor, 7=LabTech
        Department NVARCHAR(80) NOT NULL DEFAULT 'General Operations',
        MonthlySalary DECIMAL(18,2) NOT NULL,
        Shift INT NOT NULL DEFAULT 1,
        IsActive BIT NOT NULL DEFAULT 1,
        DateJoined DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO

-- 2. Table: SalaryRecords
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SalaryRecords' AND xtype='U')
BEGIN
    CREATE TABLE SalaryRecords (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        StaffMemberId INT NOT NULL FOREIGN KEY REFERENCES StaffMembers(Id) ON DELETE CASCADE,
        MonthYear NVARCHAR(20) NOT NULL,
        BaseSalary DECIMAL(18,2) NOT NULL,
        OvertimeAllowance DECIMAL(18,2) NOT NULL DEFAULT 0.00,
        Deductions DECIMAL(18,2) NOT NULL DEFAULT 0.00,
        Status INT NOT NULL DEFAULT 1, -- 1=Pending, 2=Processing, 3=Paid, 4=OnHold
        PaidDate DATETIME2 NULL,
        TransactionReference NVARCHAR(60) NOT NULL DEFAULT '',
        Remarks NVARCHAR(MAX) NULL
    );
END
GO

-- 3. Table: Patients
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Patients' AND xtype='U')
BEGIN
    CREATE TABLE Patients (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        FullName NVARCHAR(120) NOT NULL,
        AgeGender NVARCHAR(30) NULL,
        ContactNumber NVARCHAR(25) NULL,
        Department NVARCHAR(80) NOT NULL DEFAULT 'Emergency',
        Urgency INT NOT NULL DEFAULT 1,
        Status NVARCHAR(80) NOT NULL DEFAULT 'Triaged',
        MedicalNotes NVARCHAR(MAX) NULL,
        AssignedDoctorId INT NULL FOREIGN KEY REFERENCES StaffMembers(Id) ON DELETE SET NULL,
        AdmittedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        DischargedAt DATETIME2 NULL
    );
END
GO

-- Initial Seeds
INSERT INTO StaffMembers (FullName, Email, PhoneNumber, Role, Department, MonthlySalary, Shift) VALUES
('Dr. Robert Chen', 'robert.chen@carepulse.org', '+1-555-0192', 1, 'Cardiology', 9500.00, 1),
('Dr. Sarah Al-Mansoor', 'sarah.mansoor@carepulse.org', '+1-555-0193', 1, 'Neurology', 11000.00, 1),
('Dr. Emily Watson', 'emily.watson@carepulse.org', '+1-555-0194', 1, 'Pediatrics', 8800.00, 2),
('Sister Maria Santos', 'maria.santos@carepulse.org', '+1-555-0210', 2, 'ICU Ward', 5400.00, 1),
('Nurse Kevin Zhao', 'kevin.zhao@carepulse.org', '+1-555-0211', 3, 'Emergency Care', 4200.00, 3),
('Jessica Miller', 'jessica.m@carepulse.org', '+1-555-0220', 4, 'Front Desk & Admissions', 3200.00, 1),
('David Sterling', 'david.s@carepulse.org', '+1-555-0230', 5, 'Central Pharmacy', 4800.00, 1),
('Rashid Ali', 'rashid.ali@carepulse.org', '+1-555-0240', 6, 'Sanitation & Hygiene (Sweeper)', 2400.00, 1),
('Elena Rostova', 'elena.r@carepulse.org', '+1-555-0241', 6, 'Sterilization & Janitorial', 2400.00, 2);
GO
