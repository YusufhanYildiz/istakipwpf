# Implementation Plan: Job Tracking, Backup, and Export/Import Implementation

## Phase 1: Infrastructure & Refinement

- [x] Task: Database Schema Update 861243a
    - [ ] Update `DatabaseBootstrap.cs` to include `Jobs` and `Settings` tables.
    - [ ] Implement `ISettingsRepository` and `SettingsRepository`.
    - [ ] Verify: Database correctly initializes with all tables.

- [ ] Task: Customer Module Refinement
    - [ ] Update `Customer` model to include any missing fields (e.g., `Phone` mapping).
    - [ ] Update `CustomerListView.xaml` to show the "Adres" column.
    - [ ] Verify: Address is correctly displayed in the Customer DataGrid.

- [ ] Task: Conductor - User Manual Verification 'Phase 1: Infrastructure & Refinement' (Protocol in workflow.md)

## Phase 2: Job Module Core (TDD)

- [ ] Task: Job Model & Repository
    - [ ] Write Tests: Define `IJobRepository` and write tests for CRUD and customer-based queries.
    - [ ] Implement: `Job` model and `JobRepository` using Dapper.
    - [ ] Verify: All repository tests pass.

- [ ] Task: Job Service Layer
    - [ ] Write Tests: Mock repository and test business logic (validation, status updates).
    - [ ] Implement: `JobService` for data orchestration.
    - [ ] Verify: >80% coverage on service methods.

- [ ] Task: Conductor - User Manual Verification 'Phase 2: Job Module Core' (Protocol in workflow.md)

## Phase 3: Backup & Export Services

- [ ] Task: Backup & Restore Service
    - [ ] Write Tests: Verify database file copy and restore logic.
    - [ ] Implement: `BackupService` with manual/auto triggers and restore capability.
    - [ ] Verify: Successfully creates and restores backups.

- [ ] Task: Integration Services (Excel/PDF)
    - [ ] Write Tests: Verify Excel schema validation and PDF generation logic.
    - [ ] Implement: `ExcelService` (ClosedXML) and `ReportingService` (iTextSharp).
    - [ ] Verify: Bulk import/export and PDF generation work without data loss.

- [ ] Task: Conductor - User Manual Verification 'Phase 3: Backup & Export Services' (Protocol in workflow.md)

## Phase 4: UI Implementation (Material Design)

- [ ] Task: Job Management UI
    - [ ] Create `JobListViewModel` and `JobListView` (Global Task Board).
    - [ ] Implement color-coded status badges and filtering logic.
    - [ ] Create `AddEditJobDialog` using MaterialDesign DialogHost.

- [ ] Task: Settings & Notifications
    - [ ] Create `SettingsViewModel` and `SettingsView`.
    - [ ] Implement Snackbar notifications for all modules.
    - [ ] Hook into Application Exit event for the backup prompt.

- [ ] Task: Conductor - User Manual Verification 'Phase 4: UI Implementation' (Protocol in workflow.md)
