# Implementation Plan: Job Tracking, Backup, and Reporting Implementation

## Phase 1: Job Module Core (TDD)

- [ ] Task: Job Model & Repository
    - [ ] Write Tests: Define `IJobRepository` and unit tests for Job CRUD and Customer-linked queries.
    - [ ] Implement: `Job` model and `JobRepository` using Dapper.
    - [ ] Verify: All repository tests pass against the SQLite database.

- [ ] Task: Job Service Layer
    - [ ] Write Tests: Mock repository and test business logic (e.g., status transitions, date validation).
    - [ ] Implement: `JobService` for orchestration and validation.
    - [ ] Verify: >80% coverage on JobService methods.

- [ ] Task: Conductor - User Manual Verification 'Phase 1: Job Module Core' (Protocol in workflow.md)

## Phase 2: Backup & Restore Service

- [ ] Task: Backup Service Implementation
    - [ ] Write Tests: Verify database file copying and metadata generation (JSON log).
    - [ ] Implement: `BackupService` with Manual and Automatic (Exit-triggered) methods.
    - [ ] Verify: Successfully creates a restorable backup file in a target folder.

- [ ] Task: Restore Logic
    - [ ] Write Tests: Verify restoration from a backup file (file replacement logic).
    - [ ] Implement: Restore functionality within `BackupService`.
    - [ ] Verify: System state correctly reverts after a restore operation.

- [ ] Task: Conductor - User Manual Verification 'Phase 2: Backup & Restore Service' (Protocol in workflow.md)

## Phase 3: Integration & Reporting Services

- [ ] Task: Excel Import/Export Service
    - [ ] Write Tests: Verify schema validation for Customer and Job imports.
    - [ ] Implement: `ExcelService` using ClosedXML for bulk data processing.
    - [ ] Verify: Successful data round-trip (Export -> Modify -> Import).

- [ ] Task: PDF Reporting Service
    - [ ] Write Tests: Verify PDF file generation logic.
    - [ ] Implement: `ReportingService` using iTextSharp to generate "Customer Job History" reports.
    - [ ] Verify: Produces a valid PDF with correct customer and job data.

- [ ] Task: Conductor - User Manual Verification 'Phase 3: Integration & Reporting Services' (Protocol in workflow.md)

## Phase 4: UI Implementation & Refinement

- [ ] Task: Job Management UI
    - [ ] Create `JobListViewModel` and `JobListView` (Global Task Board).
    - [ ] Implement filtering logic (Customer, Status, Date) in ViewModel.
    - [ ] Create `AddEditJobDialog` using Material Design components.

- [ ] Task: Settings & Backup UI
    - [ ] Create `SettingsViewModel` and `SettingsView`.
    - [ ] Implement UI for selecting backup locations and viewing backup history.
    - [ ] Hook into Application Exit event to trigger the backup prompt.

- [ ] Task: UI Refinements
    - [ ] Update `CustomerListView` DataGrid to include the "Adres" column.
    - [ ] Finalize Side Navigation with "İş Takibi" and "Ayarlar".

- [ ] Task: Conductor - User Manual Verification 'Phase 4: UI Implementation & Refinement' (Protocol in workflow.md)
