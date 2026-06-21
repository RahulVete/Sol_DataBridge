# Sol_DataBridge

## Overview

Sol_DataBridge is a high-performance .NET 8 Web API designed to import large JSON files into SQL Server using a scalable, enterprise-grade data ingestion pipeline.

The application performs the following operations:

1. Upload JSON file via REST API.
2. Store uploaded JSON temporarily on the server.
3. Parse JSON data.
4. Generate GUID-based relationships between entities.
5. Normalize hierarchical JSON into relational staging tables.
6. Perform high-speed SQL Bulk Copy operations.
7. Execute SQL Stored Procedures to move data from staging tables to final tables.
8. Perform validation checks.
9. Prevent duplicate records.
10. Track import status.
11. Log validation and technical errors.
12. Clean up temporary files after successful import.
13. Support transactional processing with rollback capability.

---

# Business Flow

```text
Upload JSON
     │
     ▼
Store Temporary File
     │
     ▼
Parse JSON
     │
     ▼
Generate Entity Relationships
     │
     ▼
Validation
     │
     ▼
Load Staging Tables
     │
     ▼
SQL Bulk Copy
     │
     ▼
Stored Procedure
     │
     ▼
Move To Final Tables
     │
     ▼
Delete Temporary File
     │
     ▼
Commit Transaction
```

---

# JSON Hierarchy

The application processes nested JSON structures similar to:

```text
Invoice
│
├── ItemList
│
└── AssortmentDetail
     │
     └── PackingInfo
            │
            └── PairDetail
```

---

# Solution Architecture

```text
┌───────────────────────────┐
│        Swagger UI         │
└─────────────┬─────────────┘
              │
              ▼
┌───────────────────────────┐
│     ImportController      │
└─────────────┬─────────────┘
              │
              ▼
┌───────────────────────────┐
│      ImportService        │
└─────────────┬─────────────┘
              │
              │
     ┌────────┼─────────┐
     ▼        ▼         ▼
 FileService  Parser   Validation
     │                  Service
     │
     ▼
Temp JSON File
     │
     ▼
Repository Layer
     │
     ▼
BulkCopy Service
     │
     ▼
SQL Server
     │
     ▼
Stored Procedure
     │
     ▼
Final Tables
```

---

# Project Structure

```text
Sol_DataBridge
│
├── Controllers
│     └── ImportController.cs
│
├── Services
│     │
│     ├── ImportService.cs
│     ├── FileService.cs
│     ├── JsonStreamingParserService.cs
│     ├── BulkCopyService.cs
│     ├── ValidationService.cs
│     ├── ErrorLoggerService.cs
│     └── ImportStatusService.cs
│
├── Services
│    └── Interfaces
│
├── Repositories
│    ├── ImportRepository.cs
│    └── Interfaces
│
├── Data
│    └── SqlConnectionFactory.cs
│
├── Models
│    ├── DTOs
│    └── Entities
│
├── Helpers
│    └── DataTableFactory.cs
│
├── appsettings.json
│
└── Program.cs
```

---

# Design Patterns Used

## 1. Repository Pattern

Purpose:

- Encapsulates all database access logic.
- Separates business logic from data access logic.

Example:

```csharp
IImportRepository
ImportRepository
```

Benefits:

- Easier maintenance.
- Better testability.
- Cleaner architecture.

---

## 2. Dependency Injection (DI)

Purpose:

- Loosely couples components.
- Managed by .NET Core built-in IoC container.

Example:

```csharp
builder.Services.AddScoped<IImportService, ImportService>();
```

Benefits:

- Improved testability.
- Better maintainability.
- Easier mocking.

---

## 3. Service Layer Pattern

Purpose:

Contains business logic.

Examples:

```text
ImportService
ValidationService
ErrorLoggerService
ImportStatusService
```

Benefits:

- Separation of concerns.
- Reusable business logic.

---

## 4. Factory Pattern

Purpose:

Creates DataTable objects dynamically.

Example:

```csharp
DataTableFactory.CreateInvoiceTable()
```

Benefits:

- Centralized object creation.
- Reduces duplicate code.

---

## 5. Transaction Script Pattern

Purpose:

Ensures all database operations execute as a single unit.

Example:

```csharp
Begin Transaction

Validation

Bulk Copy

Stored Procedure

Commit

Rollback on Error
```

Benefits:

- Data consistency.
- Atomic processing.

---

# Database Architecture

## Staging Tables

```text
tbl_Invoice_Stg
tbl_ItemList_Stg
tbl_AssortmentDetail_Stg
tbl_PackingInfo_Stg
tbl_PairDetail_Stg
```

Purpose:

- Temporary storage.
- Faster bulk loading.
- Data validation layer.

---

## Final Tables

```text
tbl_Invoice
tbl_ItemList
tbl_AssortmentDetail
tbl_PackingInfo
tbl_PairDetail
```

Purpose:

- Permanent storage.
- Business reporting.
- Application consumption.

---

## Audit Tables

### Import Tracking

```text
tblImportBatch
```

Stores:

- Import status
- File information
- Success counts
- Failure counts

---

### Validation Errors

```text
tblValidationError
```

Stores:

- Business validation failures
- Rejected records

---

### Error Logs

```text
tblErrorLogger
```

Stores:

- Technical exceptions
- Stack traces
- Root cause analysis

---

# Performance Features

## SQL Bulk Copy

Used instead of row-by-row inserts.

Benefits:

- Extremely fast inserts.
- Supports lakhs of records.
- Optimized SQL Server loading.

---

## Batch Processing

Configured batch size:

```text
10,000 Records
```

Benefits:

- Reduced memory usage.
- Better scalability.

---

## Staging Architecture

Benefits:

- Faster ingestion.
- Better validation.
- Easier troubleshooting.

---

# Error Handling Strategy

```text
Try
 │
 ▼
Process Import
 │
 ▼
Commit
```

If any exception occurs:

```text
Catch
 │
 ▼
Log Error
 │
 ▼
Rollback Transaction
 │
 ▼
Return Error Response
```

---

# Transaction Management

All critical operations run inside a single SQL transaction.

```text
Validation
     +
Bulk Copy
     +
Stored Procedure
     +
Import Tracking
     +
Error Logging
```

Commit only when all steps succeed.

Otherwise:

```text
Rollback Entire Transaction
```

---

# Security Features

- Parameterized SQL.
- Dependency Injection.
- Configuration-based connection strings.
- HTTPS enabled.
- Swagger for testing.

---

# Technology Stack

| Component | Technology |
|------------|------------|
| Framework | .NET 8 |
| API | ASP.NET Core Web API |
| Database | SQL Server |
| Data Access | Dapper |
| Bulk Loading | SqlBulkCopy |
| Documentation | Swagger |
| IDE | Visual Studio 2022 |
| DB Tool | SQL Server Management Studio |

---

# Future Enhancements

## Phase 5

### True Streaming Parser

Current:

```text
JSON → Object
```

Future:

```text
JSON Stream
     │
     ▼
Batch Processing
```

Using:

```csharp
Utf8JsonReader
```

Benefits:

- Handle millions of records.
- Reduced memory footprint.

---


---

# Author

Project Name:

```text
Sol_DataBridge
```

Purpose:

```text
Enterprise-grade JSON to SQL Server Data Integration Platform
```