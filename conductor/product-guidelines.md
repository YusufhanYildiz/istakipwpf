# UI / UX Guidelines

## Visual Aesthetic
- **Professional & Minimalist:** Use the WPFUI (Lepoco) library to maintain a clean, modern Windows-native look.
- **Whitespace & Typography:** Prioritize readability with generous spacing and standard system fonts.
- **Color Palette:** Use standard WPFUI themes. Reserve accent colors for high-value information like status badges (Waiting: Amber, In Progress: Blue, Completed: Green, Cancelled: Red).

## Interaction Patterns
- **Navigation:** Side navigation menu for quick switching between Customers, Jobs, and Settings.
- **Data Grids:** All grids must support sorting and column-based filtering.
- **Global Search:** Implement a "search-as-you-type" functionality for all main modules.

# Functional Guidelines

## Data Management
- **Soft Delete:** Customers and Jobs should not be permanently deleted. Use an `IsDeleted` flag to hide them from standard views.
- **Validation:** 
  - Phone numbers must follow a standard format.
  - Required fields (e.g., Customer Name, Job Title) must be validated before saving.
- **Concurrency:** Ensure the SQLite connection is managed centrally to prevent database locks.

## Backup & Safety
- **Exit Strategy:** On application close, prompt the user with: "Automatically backup on exit?". Provide a "Don't show again" option to remember the user's preference in settings.
- **File Operations:** All import/export operations (Excel/PDF) must be wrapped in exception handling with user-friendly error messages.

## Architectural Standards
- **Strict MVVM:** No business logic in View code-behind files. Use commands and data binding exclusively.
- **Repository Pattern:** Abstract Dapper queries into repository classes for maintainability.
- **Localization:** All UI strings must be stored in Resource files, defaulting to Turkish (TR).
