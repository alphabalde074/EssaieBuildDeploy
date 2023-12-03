# Migrations

| Action | Package Manager Console <br> (Visual Studio) | .NET Core CLI <br> (Visual Studio Code)
| ----- | ----- | ----- |
| Add migration | `Add-Migration <MigrationName> -StartupProject Fujitsu.CvQc.API -Project Fujitsu.CvQc.Data -Context Fujitsu.CvQc.Data.DataContext` | `dotnet ef migrations add <MIGRATION NAME> --startup-project Fujitsu.CvQc.API --project Fujitsu.CvQc.Data --context Fujitsu.CvQc.Data.DataContext`
| Update database | `Update-Database -StartupProject Fujitsu.CvQc.API -Project Fujitsu.CvQc.Data -Context Fujitsu.CvQc.Data.DataContext` | `dotnet ef database update --startup-project Fujitsu.CvQc.API --project Fujitsu.CvQc.Data --context Fujitsu.CvQc.Data.DataContext`
| Remove last migration | `Remove-Migration -StartupProject Fujitsu.CvQc.API -Project Fujitsu.CvQc.Data -Context Fujitsu.CvQc.Data.DataContext` | `dotnet ef migrations remove --startup-project Fujitsu.CvQc.API --project Fujitsu.CvQc.Data --context Fujitsu.CvQc.Data.DataContext`

References: 
- https://learn.microsoft.com/en-us/ef/core/cli/powershell
- https://learn.microsoft.com/en-us/ef/core/cli/dotnet
- https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/managing


## How to manage database migrations
We use Entity Framework Core CLI to manage migrations.
### Before adding a migration
- [ ] Entire solution must be buildable (0 Errors)
- [ ] Valid connection string in data context [^1]
- [ ] Disable GlobalProtect [^2]
- [ ] Review the changes made to Entities in the Data Context

### Adding a migration

#### .NET Core CLI (Visual Studio Code)
1. Open a terminal on *Fujitsu.CvQc.API*.
2. Entrer `dotnet ef migrations add <MIGRATION NAME> --startup-project Fujitsu.CvQc.API --project Fujitsu.CvQc.Data --context Fujitsu.CvQc.Data.DataContext` in the terminal and change `<MIGRATION NAME>` for a more descriptive name (PascalCase, no spaces).
3. Wait for the confirmation in the terminal.

#### Package Manager Console (Visual Studio)
1. Open *Package Manager Console* with *Fujitsu.CvQc.API* as default project.
2. Entrer `Add-Migration <MigrationName> -StartupProject Fujitsu.CvQc.API -Project Fujitsu.CvQc.Data -Context Fujitsu.CvQc.Data.DataContext` in the *Package Manager Console* and change `<MigrationName>` for a more descriptive name (PascalCase, no spaces).
3. Wait for the confirmation in the *Package Manager Console*.


If everything has been done correctly, there should be a new file in folder [Fujitsu.CvQc.Data/Migrations](#) starting with the year, month, day and ending with the name of the migration.
The generated file contains the code to apply and remove the migration.
==If both methods are empty==, it means there are no changes needed or there is something wrong. Please ==remove the migration== (with the command)
### After adding a migration
 Review the code generated for the migration.
 
 > :warning: Be aware that some migrations may result in a loss of data

 To avoid losing data, you may need to manually adjust the migration code in some cases, but be careful and check if the same result can be achieve by modifying the entities or data context first.

 > ℹ️ The migrations are not applied to the database automatically

 ### Updating the database (applying migrations)
 > :warning: If you are using the same database as other people working on the project, it is recommended to apply the migrations on a local database to test them first. If you apply migrations to the shared database while someone else access it, it can cause severe issues like data corruption.
 
 ### Removing a migration
 If a migration has already been applied to the database, you will need to force it to be removed. Add `--force` or `-Force` to the command.

[^1]: This includes authentication to the database with modification access
[^2]: As of May 25th 2023, Fujitsu's VPN Firewall seems to block the access to the database