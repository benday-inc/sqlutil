# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**sqlutil** (`Benday.SqlUtil`) is a .NET 10.0 global CLI tool for SQL Server database introspection, comparison, data export, and connection management. It is published to NuGet as a global tool.

## Build & Test Commands

All commands run from the repo root. The solution file is at `Benday.SqlUtils/Benday.SqlUtils.slnx`.

```bash
# Build entire solution
dotnet build Benday.SqlUtils/

# Run all tests
dotnet test Benday.SqlUtils/test/Benday.SqlUtils.UnitTests/

# Run a single test method
dotnet test Benday.SqlUtils/test/Benday.SqlUtils.UnitTests/ --filter "FullyQualifiedName~TestMethodName"

# Pack CLI tool as NuGet package
dotnet pack Benday.SqlUtils/src/Benday.SqlUtils.SqlUtilCli/ --output ./artifacts
```

## Install / Uninstall (PowerShell)

```powershell
# Build and install as global tool
.\Benday.SqlUtils\install.ps1

# Reinstall (rebuild + reinstall)
.\Benday.SqlUtils\install.ps1 -reinstall

# Uninstall
.\Benday.SqlUtils\uninstall.ps1
```

## Architecture

The solution has three projects:

1. **`Benday.SqlUtils.Api`** — Reusable library. Key classes:
   - `SqlServerDatabaseUtility` — Core SQL Server query execution
   - `DatabaseConnectionString` — Connection string parsing/building
   - `SqlDataExporter` — Exports query results as INSERT/MERGE scripts

2. **`Benday.SqlUtils.SqlUtilCli`** — CLI executable. Key classes:
   - `Program.cs` — Entry point using `Benday.CommandsFramework`'s `DefaultProgram`, which auto-discovers commands via `[Command]` attributes
   - `DatabaseCommandBase` — Abstract base for all commands; handles connection resolution, argument management, and DataTable display
   - `Commands/` — 19 concrete command implementations

3. **`Benday.SqlUtils.UnitTests`** — MSTest unit tests

## Command Framework Pattern

Commands use the `Benday.CommandsFramework` library:
- Decorate command classes with `[Command(Name = "commandname", ...)]`
- Override `OnExecute()` to implement logic
- Arguments are declared with fluent API in the command constructor (e.g., `Arguments.AddString("server")`, `Arguments.AddBoolean("flag")`)
- `DatabaseCommandBase.CreateDatabaseUtility()` handles resolving the connection string from named connections or direct arguments

## Connection String Storage

Named connections are stored in `appsettings.json` under key `connection:{name}`. The default connection uses key `connection:default`. At runtime, commands accept either `/connectionname <name>` (looks up saved connection) or direct `/server`, `/database`, `/username`, `/password` arguments.

## Adding a New Command

1. Create a class in `Benday.SqlUtils.SqlUtilCli/Commands/` inheriting `DatabaseCommandBase`
2. Add `[Command]` attribute with `Name`, `Description`, `IsAsync = false` etc.
3. Declare arguments in the constructor
4. Override `OnExecute()` — call `CreateDatabaseUtility()` for DB access, use `WriteDataTable()` to display results

## CI/CD

`.github/workflows/dotnet.yml` builds, tests, packs, and publishes to NuGet.org on pushes to `master`. Requires `NUGET_API_KEY` secret in the `nuget-deploy` environment.

## Sample Data

`misc/sample-data.sql` and `misc/sample-data-v2.sql` contain scripts for creating a sample SQL Server database used for manual testing.
