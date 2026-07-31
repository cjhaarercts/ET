# Quick Fix Script for AWS Compilation Error
# Run this on the EC2 server with PowerShell as Administrator

Write-Host "=== Fixing AWS Compilation Error ===" -ForegroundColor Cyan
Write-Host ""

# Step 1: Check if AWS folder exists
Write-Host "Step 1: Checking for AWS folder..." -ForegroundColor Yellow
$awsPath = "C:\inetpub\wwwroot\App_Code\AWS"
if (Test-Path $awsPath) {
    Write-Host "  ⚠️  AWS folder found! Removing..." -ForegroundColor Red
    Remove-Item $awsPath -Recurse -Force
    Write-Host "  ✅ AWS folder removed" -ForegroundColor Green
} else {
    Write-Host "  ✅ AWS folder not found (good)" -ForegroundColor Green
}
Write-Host ""

# Step 2: List current App_Code files
Write-Host "Step 2: Current App_Code files:" -ForegroundColor Yellow
Get-ChildItem "C:\inetpub\wwwroot\App_Code" -Recurse -Filter "*.cs" | ForEach-Object {
    $relativePath = $_.FullName.Replace("C:\inetpub\wwwroot\", "")
    Write-Host "  ✅ $relativePath" -ForegroundColor Green
}
Write-Host ""

# Step 3: Stop IIS
Write-Host "Step 3: Stopping IIS..." -ForegroundColor Yellow
iisreset /stop
Write-Host "  ✅ IIS stopped" -ForegroundColor Green
Write-Host ""

# Step 4: Clear ASP.NET temporary files
Write-Host "Step 4: Clearing ASP.NET temporary files..." -ForegroundColor Yellow
$tempPath64 = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Temporary ASP.NET Files"
$tempPath32 = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\Temporary ASP.NET Files"

if (Test-Path $tempPath64) {
    Write-Host "  Clearing 64-bit temp files..." -ForegroundColor Gray
    Remove-Item "$tempPath64\*" -Recurse -Force -ErrorAction SilentlyContinue
    Write-Host "  ✅ 64-bit temp files cleared" -ForegroundColor Green
}

if (Test-Path $tempPath32) {
    Write-Host "  Clearing 32-bit temp files..." -ForegroundColor Gray
    Remove-Item "$tempPath32\*" -Recurse -Force -ErrorAction SilentlyContinue
    Write-Host "  ✅ 32-bit temp files cleared" -ForegroundColor Green
}
Write-Host ""

# Step 5: Start IIS
Write-Host "Step 5: Starting IIS..." -ForegroundColor Yellow
iisreset /start
Write-Host "  ✅ IIS started" -ForegroundColor Green
Write-Host ""

# Step 6: Touch web.config to force recompilation
Write-Host "Step 6: Forcing recompilation..." -ForegroundColor Yellow
$webConfigPath = "C:\inetpub\wwwroot\web.config"
if (Test-Path $webConfigPath) {
    (Get-Item $webConfigPath).LastWriteTime = Get-Date
    Write-Host "  ✅ web.config touched" -ForegroundColor Green
} else {
    Write-Host "  ⚠️  web.config not found at $webConfigPath" -ForegroundColor Yellow
}
Write-Host ""

# Done
Write-Host "=== Fix Complete ===" -ForegroundColor Green
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Cyan
Write-Host "1. Browse to: https://yoursite.com/custlookuplnhp.aspx" -ForegroundColor White
Write-Host "2. The error should be gone" -ForegroundColor White
Write-Host "3. Test the calendar picker and timezone conversion" -ForegroundColor White
Write-Host ""
Write-Host "If you still see errors, check:" -ForegroundColor Yellow
Write-Host "  - App_Code folder for any remaining AWS files" -ForegroundColor White
Write-Host "  - IIS application pool is running" -ForegroundColor White
Write-Host "  - web.config has correct connection strings" -ForegroundColor White
Write-Host ""
