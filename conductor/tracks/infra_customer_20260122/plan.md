# Implementation Plan: Project Infrastructure & Core Customer Management

## Phase 1: Project Scaffolding & Infrastructure

- [x] Task: Create .NET 4.8 WPF Project Structure a191573
    - [ ] Initialize Visual Studio Solution and Project.
    - [ ] Install NuGet Packages: WPFUI, Dapper, System.Data.SQLite, Microsoft.Extensions.DependencyInjection.
    - [ ] Set up Folder Structure: `Models`, `ViewModels`, `Views`, `Services`, `Repositories`, `Infrastructure`.

- [x] Task: Database & DI Setup d8c7b25
    - [ ] Implement `DatabaseBootstrap` to create SQLite file and `Customers` table.
    - [ ] Set up `ServiceCollection` for Dependency Injection.
    - [ ] Create `BaseRepository` for centralized Dapper connection handling.

- [ ] Task: Conductor - User Manual Verification 'Phase 1: Project Scaffolding & Infrastructure' (Protocol in workflow.md)

## Phase 2: Core Customer Logic (TDD)

- [ ] Task: Customer Model & Repository
    - [ ] Write Tests: Define `ICustomerRepository` interface and write tests for CRUD operations.
    - [ ] Implement: `Customer` model and `CustomerRepository` using Dapper.
    - [ ] Verify: Tests pass and SQLite file contains correct schema.

- [ ] Task: Customer Service Layer
    - [ ] Write Tests: Mock repository and test business logic (e.g., phone validation, soft delete logic).
    - [ ] Implement: `CustomerService` to handle data orchestration and validation.
    - [ ] Verify: >80% coverage on service methods.

- [ ] Task: Conductor - User Manual Verification 'Phase 2: Core Customer Logic' (Protocol in workflow.md)

## Phase 3: UI Implementation (WPFUI)

- [ ] Task: Main Window & Navigation
    - [ ] Implement `MainWindow` with `WPFUI.Controls.NavigationView`.
    - [ ] Create `DashboardView` and `CustomerListView`.

- [ ] Task: Customer Management UI
    - [ ] Implement `CustomerListViewModel` with Search and Sorting.
    - [ ] Create `AddEditCustomerDialog` using WPFUI dialog components.
    - [ ] Bind View to Service and Repository via DI.

- [ ] Task: Conductor - User Manual Verification 'Phase 3: UI Implementation' (Protocol in workflow.md)
