# Specification: Job Tracking, Backup, and Reporting Implementation

## Overview
This track completes the Customer & Job Tracking System by implementing the remaining core modules: Job Management, automated Backup/Restore, and Excel/PDF reporting services. It also includes UI refinements for the existing Customer module.

## Functional Requirements

### 1. Job Tracking Module
- **Fields:** Id, CustomerId, JobTitle, Description, Status (Waiting, InProgress, Completed, Cancelled), StartDate, EndDate, CreatedDate, UpdatedDate.
- **Global Task Board:** A dedicated screen showing all jobs with columns for Customer, Title, Status, and Dates.
- **Filtering:** Filter jobs by Customer, Status, and Date Range.
- **Lifecycle:** Dynamically update job status from the UI with color-coded badges.

### 2. Backup & Restore Module
- **Database Backup:** Copy the SQLite database file to a user-defined location.
- **Automatic Trigger:** Prompt the user on application exit: "Yedekleme yapmak istiyor musunuz?".
- **Restore:** Allow users to select a backup file and restore the system state.
- **History:** Maintain a simple log/list of previous backup dates and locations.

### 3. Export / Import Services
- **Excel Integration:**
  - Bulk import Customers and Jobs from `.xlsx` files (using ClosedXML).
  - Export current views (Customer list, Job board) to Excel.
- **PDF Reporting:**
  - Generate a "Customer Job History" PDF for a selected client, listing all their associated tasks and statuses.

### 4. UI Refinement
- **Customer List:** Add an "Adres" column to the main DataGrid.
- **Navigation:** Add "İş Takibi" and "Ayarlar & Yedek" items to the side menu.

## Technical Requirements
- **Excel:** ClosedXML (.NET 4.8 compatible).
- **PDF:** iTextSharp.
- **Persistence:** SQLite via Dapper.
- **Architecture:** Strict MVVM with Dependency Injection.

## Acceptance Criteria
- [ ] Users can manage the full lifecycle of jobs linked to customers.
- [ ] Jobs can be filtered by multiple criteria on a global board.
- [ ] System prompts for backup on exit and can successfully restore from a file.
- [ ] Customers and Jobs can be imported/exported via Excel without data loss.
- [ ] A professional PDF report can be generated for any customer's job history.
- [ ] Customer DataGrid correctly displays the Address field.
