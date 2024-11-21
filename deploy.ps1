$ErrorActionPreference = "Stop"

# Build the application
Write-Host "Building application..." -ForegroundColor Green
dotnet publish -c Release -o ./publish

# Create database if it doesn't exist
Write-Host "Setting up database..." -ForegroundColor Green
$connectionString = "Server=.\SQLEXPRESS;Database=WarehouseSystem;Trusted_Connection=True;MultipleActiveResultSets=true"
$query = "IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'WarehouseSystem') CREATE DATABASE WarehouseSystem;"
Invoke-Sqlcmd -Query $query -ServerInstance ".\SQLEXPRESS"

# Apply migrations
Write-Host "Applying database migrations..." -ForegroundColor Green
dotnet ef database update

# Create default admin user
Write-Host "Creating default admin user..." -ForegroundColor Green
$query = @"
IF NOT EXISTS (SELECT * FROM Users WHERE Username = 'admin')
BEGIN
    INSERT INTO Users (Username, PasswordHash, MobileNumber, Role, CreatedBy, CreatedAt, IsDeleted)
    VALUES ('admin', '`$2a`$11`$K7C1yqNDgYkG4GUKGh/T4.C23YQBEv8Ai8.1P6q.WiVj8yZGBMKXi', '123456789', 1, 'System', GETUTCDATE(), 0)
END
"@
Invoke-Sqlcmd -Query $query -Database "WarehouseSystem" -ServerInstance ".\SQLEXPRESS"

Write-Host "`nDeployment completed successfully!" -ForegroundColor Green
Write-Host "`nDefault admin credentials:" -ForegroundColor Yellow
Write-Host "Username: admin" -ForegroundColor Yellow
Write-Host "Password: Admin123!" -ForegroundColor Yellow
Write-Host "`nTo start the application, run: dotnet WarehouseSystem.dll from the publish folder" -ForegroundColor Yellow