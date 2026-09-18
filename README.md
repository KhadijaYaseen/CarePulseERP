# 🏥 CarePulse MediCore — Enterprise Hospital Management & EMR ERP

[![.NET Core](https://img.shields.io/badge/.NET_8.0-ASP.NET_Core_Web_API-512BD4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/Language-C%23_12-239120?style=for-the-badge&logo=csharp)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![Database](https://img.shields.io/badge/Database-SQL_Server_%26_MySQL-CC292B?style=for-the-badge&logo=microsoftsqlserver)](https://www.microsoft.com/sql-server)
[![ORM](https://img.shields.io/badge/ORM-Entity_Framework_Core_8-512BD4?style=for-the-badge)](https://learn.microsoft.com/ef/core/)
[![Swagger](https://img.shields.io/badge/Documentation-Swagger_OpenAPI_v1-85EA2D?style=for-the-badge&logo=swagger)](https://swagger.io/)
[![Live Demo](https://img.shields.io/badge/Live_Sandbox-Launch_Hospital_ERP-06B6D4?style=for-the-badge)](https://khadijayaseen.github.io/demo-hospital/index.html)

> **Architected & Developed by [Khadija Yaseen](https://khadijayaseen.github.io)**  
> *Software Engineer | Enterprise Backend & Mobile Solutions*  
> [LinkedIn](https://lnkd.in/p/dVChg5BR) • [Developer Portfolio](https://khadijayaseen.github.io) • [Email](mailto:khadijayaseen36@gmail.com)

---

## 🌟 Executive Summary

**CarePulse MediCore ERP** is a full-stack, enterprise-grade Hospital Management and Electronic Medical Records (EMR) platform designed for healthcare networks. It streamlines multi-department clinical operations, real-time patient triage, staff management with complete CRUD, automated payroll disbursement ledgers, and ICU/ward bed occupancy telemetry.

The backend is built on **ASP.NET Core 8 Web API** leveraging **Entity Framework Core 8** with dual database capabilities supporting **Microsoft SQL Server** (active production provider) and **MySQL 8.0**.

---

## 🚀 Live Interactive Sandbox & Web UI

Experience the live interactive client simulation directly in your browser without local setup:  
👉 **[Launch CarePulse MediCore Interactive Sandbox](https://khadijayaseen.github.io/demo-hospital/index.html)**

---

## 🏗️ System Architecture

```mermaid
flowchart TD
    subgraph Client Tier
        WebUI[Web Browser & Admin Dashboard]
        Mobile[CarePulse Mobile App / API Consumers]
        SwaggerUI[Swagger OpenAPI Interactive UI]
    end

    subgraph ASP.NET Core 8 Web API Tier
        Router[API Gateway & Route Handling]
        StaffCtrl[StaffController.cs - HR & Roles]
        PatientsCtrl[PatientsController.cs - EMR & Triage]
        PayrollCtrl[PayrollController.cs - Salary Ledger]
        BedsCtrl[Beds / Ward Telemetry Services]
    end

    subgraph Data Access Layer
        EFCore[Entity Framework Core 8 DbContext]
    end

    subgraph Enterprise Databases (Dual Engine)
        MSSQL[(Microsoft SQL Server - LocalDB / T-SQL)]
        MySQL[(MySQL 8.0 - InnoDB Engine)]
    end

    WebUI --> Router
    Mobile --> Router
    SwaggerUI --> Router

    Router --> StaffCtrl
    Router --> PatientsCtrl
    Router --> PayrollCtrl
    Router --> BedsCtrl

    StaffCtrl --> EFCore
    PatientsCtrl --> EFCore
    PayrollCtrl --> EFCore
    BedsCtrl --> EFCore

    EFCore -. Default Provider .-> MSSQL
    EFCore -. Optional Provider .-> MySQL
```

---

## 🔑 Core Enterprise Modules

### 1. 📋 Clinical Triage & Electronic Medical Records (EMR)
* Instant patient intake with clinical priority categorizations:
  * **Routine (Level 1)**, **Priority (Level 2)**, **Urgent (Level 3)**, **Critical (Level 4)**
* Dynamic physician assignment, department routing (Emergency, Cardiology, Neurology, Pediatrics), and nurse dispatch.
* Admission timestamps, discharge workflows, and historical medical notes.

### 2. 👨‍⚕️ Comprehensive Staff Directory & Role-Based HR
* Full lifecycle CRUD operations (Add, View, Edit, Terminate/Deactivate).
* Built-in support for diverse hospital roles:
  * Doctors & Specialists
  * Head Nurses & Duty Nurses
  * Receptionists & Front-Desk
  * Pharmacists
  * Laboratory Technicians
  * Sanitation, Sweepers & Janitorial Staff
* Shift assignment tracking (Morning, Evening, Night, Rotational) and monthly compensation mapping.

### 3. 💳 Automated Payroll & Salary Disbursement Ledger
* Real-time calculation of Gross Pay, Overtime Allowances, and Tax/Deductions.
* Payment lifecycle states: `Pending`, `Processing`, `Paid`, `OnHold`.
* Bank transaction reference hashing (e.g., `TX-PAY-20260801-9041`) and instant one-click disbursement API.

### 4. 🛏️ Ward & ICU Bed Telemetry Map
* Real-time tracking of occupied, vacant, reserved, and maintenance beds across ICU Alpha, CCU, and General Wards.
* Variable ECG waveform telemetry visualizer.

---

## 🗄️ Database Architecture (Dual Engine)

CarePulse ERP is architected to seamlessly run on **Microsoft SQL Server** or **MySQL 8.0** via Entity Framework Core:

### Option A: Microsoft SQL Server (Default Configured Provider)
* Production T-SQL schema: [`Database/schema.mssql.sql`](./Database/schema.mssql.sql)
* Uses `Microsoft.EntityFrameworkCore.SqlServer`
* Connection String configured in `appsettings.json`:
  ```json
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=CarePulseHospitalDB;Trusted_Connection=True;TrustServerCertificate=True;"
  ```

### Option B: MySQL 8.0 (Cross-Platform)
* Production MySQL schema: [`Database/schema.mysql.sql`](./Database/schema.mysql.sql)
* Uses `Pomelo.EntityFrameworkCore.MySql`
* High-performance InnoDB storage engine with UTF8mb4 encoding, normalized relations, and cascading deletes.

---

## 🔌 API Endpoints Summary

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/api/Staff` | Retrieve all hospital staff members with optional role/department filters |
| `POST` | `/api/Staff` | Register a new staff member (Doctor, Nurse, Receptionist, Janitor) |
| `PUT` | `/api/Staff/{id}` | Update staff profiles, salary rates, or assigned shift |
| `DELETE` | `/api/Staff/{id}` | Deactivate/remove staff records from the active register |
| `GET` | `/api/Patients` | List admitted patients filtered by urgency, department, or status |
| `POST` | `/api/Patients` | Clinical triage intake, symptoms, and doctor assignment |
| `GET` | `/api/Payroll` | Retrieve monthly hospital payroll ledger and disbursement statuses |
| `POST` | `/api/Payroll/disburse/{id}` | Execute salary disbursement and generate transaction reference |

Interactive OpenAPI documentation is exposed at: `https://localhost:5001/swagger`

---

## 💻 Local Setup & Execution Guide

### Prerequisites
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/) with C# Dev Kit
* SQL Server Express / LocalDB or MySQL 8.0 Server

### Installation Steps

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/KhadijaYaseen/CarePulseERP.git
   cd CarePulseERP
   ```

2. **Restore Dependencies:**
   ```bash
   dotnet restore
   ```

3. **Initialize the Database:**
   * **For SQL Server:** Execute [`Database/schema.mssql.sql`](./Database/schema.mssql.sql) in SQL Server Management Studio (SSMS) or via `sqlcmd`.
   * **For MySQL:** Run `mysql -u root -p < Database/schema.mysql.sql`.

4. **Run the Application:**
   ```bash
   dotnet run
   ```

5. **Explore Swagger & Web Client:**
   * **Swagger API Documentation:** `http://localhost:5000/swagger`
   * **Embedded Web Portal:** `http://localhost:5000`

---

## 👩‍💻 Author & Contact

**Khadija Yaseen**  
*BS Software Engineering — University of Management and Technology (UMT), Lahore*  
* Specialized in Enterprise .NET Core APIs, Flutter Mobile GenAI, and Modern Web Architecture.

* **Email:** [khadijayaseen36@gmail.com](mailto:khadijayaseen36@gmail.com)
* **LinkedIn:** [linkedin.com/in/khadija-yaseen09](https://lnkd.in/p/dVChg5BR)
* **Portfolio:** [khadijayaseen.github.io](https://khadijayaseen.github.io)
