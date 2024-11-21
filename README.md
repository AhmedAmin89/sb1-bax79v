# Warehouse System - Local Deployment Guide

## Prerequisites

1. SQL Server Express installed
2. .NET 6.0 SDK installed
3. PowerShell installed

## Deployment Steps

1. Open PowerShell as Administrator
2. Navigate to the project directory
3. Run the deployment script:
   ```powershell
   .\deploy.ps1
   ```

4. The script will:
   - Build the application
   - Create the database
   - Apply migrations
   - Create default admin user

5. Navigate to the publish folder:
   ```powershell
   cd publish
   ```

6. Start the application:
   ```powershell
   dotnet WarehouseSystem.dll
   ```

7. Access the application at:
   - HTTPS: https://localhost:7001
   - HTTP: http://localhost:7000

## Default Admin Credentials
- Username: admin
- Password: Admin123!

## Important Notes

1. Make sure SQL Server Express is running
2. The application uses Windows Authentication for database access
3. Default connection string uses SQL Server Express
4. Change the connection string in appsettings.Production.json if needed