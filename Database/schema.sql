-- ==========================================================
-- CarePulse MediCore — Enterprise Hospital Management ERP
-- MySQL Database Production Schema
-- Designed & Architected by Khadija Yaseen
-- ==========================================================

CREATE DATABASE IF NOT EXISTS CarePulseHospitalDB
CHARACTER SET utf8mb4 
COLLATE utf8mb4_unicode_ci;

USE CarePulseHospitalDB;

-- 1. Table: StaffMembers (Doctors, Nurses, Sweepers, Receptionists, Pharmacists)
CREATE TABLE IF NOT EXISTS StaffMembers (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    FullName VARCHAR(120) NOT NULL,
    Email VARCHAR(120) NOT NULL UNIQUE,
    PhoneNumber VARCHAR(25) NULL,
    Role INT NOT NULL COMMENT '1=Doctor, 2=HeadNurse, 3=DutyNurse, 4=Receptionist, 5=Pharmacist, 6=SweeperJanitor, 7=LabTech, 8=HospitalAdmin',
    Department VARCHAR(80) NOT NULL DEFAULT 'General Operations',
    MonthlySalary DECIMAL(18,2) NOT NULL,
    Shift INT NOT NULL DEFAULT 1 COMMENT '1=Morning, 2=Evening, 3=Night, 4=Rotational',
    IsActive BOOLEAN NOT NULL DEFAULT TRUE,
    DateJoined DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    INDEX idx_role (Role),
    INDEX idx_dept (Department),
    INDEX idx_active (IsActive)
) ENGINE=InnoDB;

-- 2. Table: SalaryRecords (Monthly Payroll, Overtime, Deductions & Disbursement)
CREATE TABLE IF NOT EXISTS SalaryRecords (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    StaffMemberId INT NOT NULL,
    MonthYear VARCHAR(20) NOT NULL,
    BaseSalary DECIMAL(18,2) NOT NULL,
    OvertimeAllowance DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    Deductions DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    Status INT NOT NULL DEFAULT 1 COMMENT '1=Pending, 2=Processing, 3=Paid, 4=OnHold',
    PaidDate DATETIME NULL,
    TransactionReference VARCHAR(60) NOT NULL DEFAULT '',
    Remarks TEXT NULL,
    FOREIGN KEY (StaffMemberId) REFERENCES StaffMembers(Id) ON DELETE CASCADE,
    INDEX idx_staff_month (StaffMemberId, MonthYear),
    INDEX idx_status (Status)
) ENGINE=InnoDB;

-- 3. Table: Beds (Ward & ICU Allocation)
CREATE TABLE IF NOT EXISTS Beds (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    BedCode VARCHAR(40) NOT NULL UNIQUE,
    WardName VARCHAR(60) NOT NULL,
    Status INT NOT NULL DEFAULT 1 COMMENT '1=Vacant, 2=Occupied, 3=Maintenance, 4=Reserved',
    CurrentPatientId INT NULL,
    DailyRate DECIMAL(18,2) NOT NULL DEFAULT 150.00,
    INDEX idx_ward (WardName),
    INDEX idx_bed_status (Status)
) ENGINE=InnoDB;

-- 4. Table: Patients (EMR & Clinical Triage)
CREATE TABLE IF NOT EXISTS Patients (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    FullName VARCHAR(120) NOT NULL,
    AgeGender VARCHAR(30) NULL,
    ContactNumber VARCHAR(25) NULL,
    Department VARCHAR(80) NOT NULL DEFAULT 'Emergency',
    Urgency INT NOT NULL DEFAULT 1 COMMENT '1=Routine, 2=Priority, 3=Urgent, 4=Critical',
    Status VARCHAR(80) NOT NULL DEFAULT 'Triaged',
    MedicalNotes TEXT NULL,
    AssignedDoctorId INT NULL,
    AssignedNurseId INT NULL,
    BedId INT NULL,
    AdmittedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    DischargedAt DATETIME NULL,
    FOREIGN KEY (AssignedDoctorId) REFERENCES StaffMembers(Id) ON DELETE SET NULL,
    FOREIGN KEY (AssignedNurseId) REFERENCES StaffMembers(Id) ON DELETE SET NULL,
    FOREIGN KEY (BedId) REFERENCES Beds(Id) ON DELETE SET NULL,
    INDEX idx_patient_status (Status),
    INDEX idx_urgency (Urgency)
) ENGINE=InnoDB;

-- ==========================================================
-- Initial Production Seed Data
-- ==========================================================

-- Insert Doctors, Nurses, Receptionists, Sweepers
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

-- Insert Sample Salary Records for Current Month (August 2026)
INSERT INTO SalaryRecords (StaffMemberId, MonthYear, BaseSalary, OvertimeAllowance, Deductions, Status, PaidDate, TransactionReference) VALUES
(1, 'August 2026', 9500.00, 500.00, 0.00, 3, '2026-08-01 10:00:00', 'TX-PAY-20260801-9041'),
(2, 'August 2026', 11000.00, 800.00, 0.00, 3, '2026-08-01 10:00:00', 'TX-PAY-20260801-9042'),
(3, 'August 2026', 8800.00, 0.00, 0.00, 1, NULL, ''),
(4, 'August 2026', 5400.00, 300.00, 0.00, 3, '2026-08-01 10:00:00', 'TX-PAY-20260801-9043'),
(5, 'August 2026', 4200.00, 250.00, 0.00, 1, NULL, ''),
(6, 'August 2026', 3200.00, 0.00, 0.00, 3, '2026-08-01 10:00:00', 'TX-PAY-20260801-9044'),
(7, 'August 2026', 4800.00, 0.00, 0.00, 1, NULL, ''),
(8, 'August 2026', 2400.00, 150.00, 0.00, 3, '2026-08-01 10:00:00', 'TX-PAY-20260801-9045'),
(9, 'August 2026', 2400.00, 100.00, 0.00, 1, NULL, '');
