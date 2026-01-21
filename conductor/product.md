# Initial Concept
Develop a fully local, offline-capable Customer & Job Tracking System using .NET Framework 4.8, WPF (MVVM), SQLite, Dapper, and WPFUI (Lepoco). The system will include Customer Management, Job/Work Tracking, and Backup/Restore modules with Excel/PDF import/export capabilities.

# Product Vision
A professional, privacy-focused desktop application designed for small business owners and solopreneurs who require absolute data ownership and offline reliability.

# Target Users
- **Small Business Owners / Solopreneurs:** Users seeking a simple yet robust tool for CRM and job management without recurring cloud costs.
- **Technical Service Providers:** Professionals needing to link specific work details, statuses, and history to individual customers.

# Key Goals
- **Data Sovereignty:** All data is stored in a local SQLite database, ensuring business intelligence never leaves the user's machine.
- **High Operational Efficiency:** A dashboard-centric UI that provides immediate insights into pending jobs and quick-action triggers for daily tasks.
- **Professional Portability:** Enabling seamless data transfer and reporting through standardized Excel imports and polished PDF exports.

# Functional Scope
- **Customer Management:** Full CRUD operations with search, soft-delete, and schema-validated Excel imports.
- **Job Tracking:** Lifecycle management of tasks (Waiting to Completed) with color-coded status indicators and date-range filtering.
- **Automated Data Protection:** A robust backup service that prompts users on exit and supports scheduled maintenance to prevent data loss.
- **Reporting & Integration:** High-quality PDF generation for customer job history and Excel integration for bulk data management.