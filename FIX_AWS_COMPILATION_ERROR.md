# How to Fix "Amazon namespace not found" Error

## Problem
IIS/ASP.NET has cached the old AWS files even though they've been deleted from the source code.

## Solution: Clear ASP.NET Temporary Files

### On the EC2 Server:

**Option 1: Delete Temporary ASP.NET Files (Recommended)**
```powershell
# Stop IIS
iisreset /stop

# Delete ASP.NET temporary compiled files
Remove-Item -Path "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Temporary ASP.NET Files\*" -Recurse -Force

# If you also have 32-bit:
Remove-Item -Path "C:\Windows\Microsoft.NET\Framework\v4.0.30319\Temporary ASP.NET Files\*" -Recurse -Force

# Start IIS
iisreset /start
```

**Option 2: Touch web.config (Faster)**
```powershell
# Just update the web.config timestamp to force recompilation
$webConfig = "C:\inetpub\wwwroot\web.config"
(Get-Item $webConfig).LastWriteTime = Get-Date
```

**Option 3: Recycle App Pool**
```powershell
# In IIS Manager: 
# Application Pools → Select your pool → Recycle
# Or via command line:
Import-Module WebAdministration
Restart-WebAppPool -Name "DefaultAppPool"  # Replace with your pool name
```

---

## Verification

After clearing the cache, browse to:
```
https://yoursite.com/custlookuplnhp.aspx
```

The error should be gone and the page should load.

---

## Why This Happened

ASP.NET Web Sites compile pages **on-demand** at runtime. When a page is first accessed:
1. ASP.NET compiles all `.cs` files in `App_Code`
2. Stores compiled DLLs in `Temporary ASP.NET Files`
3. Reuses those DLLs until something changes

When we deleted the AWS folder, ASP.NET didn't know to recompile because:
- The files were deleted directly (not through IIS)
- The compiled DLLs still reference the old AWS code
- IIS cache needs to be cleared

---

## Prevention

To avoid this in the future:
1. Always recycle the app pool after file changes
2. Or touch `web.config` to trigger recompilation
3. Or use the "Rebuild" feature in Visual Studio

---

## Alternative: Deploy Without AWS Files

If you're deploying from your dev machine, make sure these files are NOT in `App_Code`:
```
❌ App_Code\AWS\AwsSecretsHelper.cs
❌ App_Code\AWS\AwsSesEmailService.cs
❌ App_Code\AWS\CloudWatchLogger.cs
```

These files should either be:
- Completely deleted, OR
- Moved to `App_Code_AWS_DISABLED` (outside App_Code)

---

## What's in App_Code Now

Only these files should be in `App_Code`:
```
✅ App_Code\EmailHelper.cs
✅ App_Code\Helpers\TimezoneHelper.cs
✅ App_Code\Models\Customer.cs
✅ App_Code\Repositories\CustomerRepository.cs
✅ App_Code\Services\AgentEmailService.cs
```

All of these are AWS-free and will work without any NuGet packages.

---

## Quick Fix Commands

**Run these on the EC2 server:**
```powershell
# Method 1: Clear cache and restart (most reliable)
iisreset /stop
Remove-Item "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Temporary ASP.NET Files\*" -Recurse -Force
iisreset /start

# Method 2: Just restart IIS (faster, may work)
iisreset

# Method 3: Touch web.config (fastest)
(Get-Item "C:\inetpub\wwwroot\web.config").LastWriteTime = Get-Date
```

Try Method 3 first. If that doesn't work, use Method 1.

---

## Expected Result

After clearing the cache, you should see:
- ✅ Page loads successfully
- ✅ Calendar picker appears
- ✅ Can edit customer records
- ✅ Timezone conversion works
- ✅ No AWS errors

---

## If Problem Persists

If you still see the error after clearing cache:

1. **Check if AWS files still exist on server:**
   ```powershell
   Get-ChildItem "C:\inetpub\wwwroot\App_Code\AWS" -ErrorAction SilentlyContinue
   ```
   If this returns files, delete them:
   ```powershell
   Remove-Item "C:\inetpub\wwwroot\App_Code\AWS" -Recurse -Force
   ```

2. **Check for multiple web.config files:**
   ```powershell
   Get-ChildItem "C:\inetpub\wwwroot" -Filter "web.config" -Recurse
   ```

3. **Verify App_Code contents:**
   ```powershell
   Get-ChildItem "C:\inetpub\wwwroot\App_Code" -Recurse -Filter "*.cs"
   ```

4. **Check IIS bindings and paths** in IIS Manager

---

## Summary

**Problem:** ASP.NET cached old compiled code that references AWS SDK  
**Solution:** Clear `Temporary ASP.NET Files` folder and restart IIS  
**Quick Fix:** Run `iisreset` or touch web.config  

The AWS files are already removed from your source code. You just need to clear the server cache.
