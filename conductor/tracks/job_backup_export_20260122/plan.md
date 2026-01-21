# Implementation Plan: Job Tracking, Backup, and Export/Import Implementation

## Phase 1: Infrastructure & Refinement [checkpoint: 3100b22]

- [x] Task: Database Schema Update 861243a
    - [x] Update `DatabaseBootstrap.cs` to include `Jobs` and `Settings` tables.
    - [x] Implement `ISettingsRepository` and `SettingsRepository`.
    - [x] Verify: Database correctly initializes with all tables.

- [x] Task: Customer Module Refinement 8675f61
    - [x] Update `Customer` model to include any missing fields (e.g., `Phone` mapping).
    - [x] Update `CustomerListView.xaml` to show the "Adres" column.
    - [x] Verify: Address is correctly displayed in the Customer DataGrid.

- [x] Task: Conductor - User Manual Verification 'Phase 1: Infrastructure & Refinement' (Protocol in workflow.md) 3100b22

## Phase 2: Job Module Core (TDD) [checkpoint: bcef801]

- [x] Task: Job Model & Repository 62bdf57
    - [x] Write Tests: Define `IJobRepository` and write tests for CRUD and customer-based queries.
    - [x] Implement: `Job` model and `JobRepository` using Dapper.
    - [x] Verify: All repository tests pass.

- [x] Task: Job Service Layer 0961180
    - [x] Write Tests: Mock repository and test business logic (validation, status updates).
    - [x] Implement: `JobService` for data orchestration.
    - [x] Verify: >80% coverage on service methods.

- [x] Task: Conductor - User Manual Verification 'Phase 2: Job Module Core' (Protocol in workflow.md) bcef801

## Phase 3: Backup & Export Services

- [x] Task: Backup & Restore Service 2dbb2c9
    - [ ] Write Tests: Verify database file copy and restore logic.
    - [ ] Implement: `BackupService` with manual/auto triggers and restore capability.
    - [ ] Verify: Successfully creates and restores backups.

- [x] Task: Integration Services (Excel/PDF) 57e2e95
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
