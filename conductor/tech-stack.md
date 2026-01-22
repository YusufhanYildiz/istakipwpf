# Technology Stack

## Core Framework
- **Runtime:** .NET Framework 4.8
- **UI Platform:** Windows Presentation Foundation (WPF)
- **Architecture Pattern:** Model-View-ViewModel (MVVM)

## User Interface (UI/UX)
- **Component Library:** [Material Design In Xaml Toolkit](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit) - Material Design for WPF.
- **Icons:** Material Design Icons.
- **Localization:** Standard .NET Resources (.resx) for Turkish (TR) and English (EN) support.

## Data Persistence
- **Database Engine:** [SQLite](https://www.sqlite.org/) - Zero-configuration, serverless, file-based SQL engine.
- **Micro-ORM:** [Dapper](https://github.com/DapperLib/Dapper) - High-performance object mapper for .NET.
- **Connection Management:** System.Data.SQLite (NuGet package).
- **Location Services:** Custom offline `LocationService` containing comprehensive datasets for Turkey (81 provinces and their districts).
- **Security:** `System.Security.Cryptography.ProtectedData` (DPAPI) for secure local credential storage.

## File Integration & Reporting
- **Excel Handling:** [ClosedXML](https://github.com/ClosedXML/ClosedXML) - For robust Excel (.xlsx) import and export.
- **PDF Generation:** [iTextSharp](https://github.com/itext/itextsharp) (LGPL/MPL version) - Proven PDF library compatible with .NET 4.8.
- **JSON Serialization:** `System.Text.Json` (via NuGet) for configuration and backup metadata.

## Development & Tooling
- **Dependency Injection:** Microsoft.Extensions.DependencyInjection (compatible with .NET 4.8).
- **Project Management:** Gemini CLI Conductor.
