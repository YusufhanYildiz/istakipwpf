# Specification: Job Tracking, Backup, and Export/Import Implementation

## Overview
This track focuses on implementing the remaining core functionality of the Customer & Job Tracking System. This includes the complete Job Tracking module, a robust Backup/Restore system, Excel import/export capabilities, and professional PDF reporting. It also addresses UI refinements requested for the Customers module.

## Functional Requirements

### 1. Job Tracking Module
- **Core CRUD:** Full management of Job records linked to Customers.
- **Statuses:** Bekliyor (Orange), Devam Ediyor (Blue), Tamamlandı (Green).
- **Filtering:** Filter the global job board by Customer name, date range, and status.
- **UI:** Global Task Board using DataGrid with color-coded status badges.

### 2. Backup & Restore Module
- **Manuel & Auto:** Support for manual backups and optional prompt on application exit.
- **Restore:** Ability to restore the SQLite database from a selected `.db` backup file.
- **History:** Backup history stored in the `Settings` table.
- **Settings:** Configurable backup location and frequency settings.

### 3. Integration Services (Excel/PDF)
- **Excel Import:** Bulk import Customers and Jobs from `.xlsx` files using ClosedXML with strict schema validation (All-or-Nothing).
- **Excel Export:** Export the currently filtered list of Customers or Jobs to Excel.
- **PDF Reporting:** Generate a "Customer Job History" PDF using iTextSharp for a selected client.

### 4. UI Refinements
- **Customer List:** Add "Adres" column to the main DataGrid.
- **Dialogs:** Implement MaterialDesign DialogHost for adding/editing Jobs and managing Settings.
- **Notifications:** Use Snackbar for operation success/error feedback.

## Technical Requirements
- **Persistence:** Dapper for Jobs and Settings tables.
- **Libraries:** ClosedXML (Excel), iTextSharp (PDF).
- **Localization:** Turkish (TR) as default for all UI labels and messages.

## Acceptance Criteria
- [ ] Users can manage jobs and track statuses with clear visual feedback.
- [ ] Database backups can be created manually and on exit.
- [ ] System successfully restores data from a backup file.
- [ ] Excel import validates data and prevents partial/broken imports.
- [ ] PDF reports are generated with professional formatting.
- [ ] Address column is visible and correctly populated in the Customer list.
