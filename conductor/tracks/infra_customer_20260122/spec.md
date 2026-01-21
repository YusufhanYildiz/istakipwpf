# Specification: Project Infrastructure & Core Customer Management

## Overview
This track focuses on setting up the foundational technical architecture for the Customer & Job Tracking System and implementing the first functional module: Customer Management.

## Objectives
1.  Establish the .NET 4.8 WPF project structure with MVVM.
2.  Configure SQLite database connectivity using Dapper.
3.  Implement a centralized Dependency Injection (DI) container.
4.  Develop the Customer Management module with full CRUD and search capabilities.
5.  Ensure all data operations support "Soft Delete" functionality.

## Technical Requirements
- **Framework:** .NET Framework 4.8.
- **UI:** WPFUI (Lepoco) for modern styling.
- **Data Access:** Dapper as the Micro-ORM.
- **Persistence:** SQLite local file (`AppData/database.db`).
- **Architecture:** Strict MVVM (View -> ViewModel -> Service -> Repository).

## Functional Requirements
- **Customer Fields:** Id, FirstName, LastName, PhoneNumber, Address, CreatedDate, UpdatedDate, IsDeleted.
- **Search:** Filter customers by Name (First/Last) or Phone number in real-time.
- **Soft Delete:** A `Delete` operation sets `IsDeleted = 1` instead of removing the row.
- **UI:** A DataGrid showing all non-deleted customers with sorting and row actions.

## Acceptance Criteria
- [ ] Project compiles and launches a WPFUI-styled window.
- [ ] Database is automatically created on first run with the `Customers` table.
- [ ] Users can add, edit, and search for customers.
- [ ] Deleted customers are hidden from the UI but remain in the database.
- [ ] Unit tests cover >80% of the Business Logic (ViewModels and Services).
