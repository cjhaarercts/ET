# 🔧 IMMEDIATE FIX REQUIRED - AWS Compilation Error

## ⚠️ Current Error
```
CS0246: The type or namespace name 'Amazon' could not be found
Source File: App_Code\AWS\CloudWatchLogger.cs
```

---

## ✅ SOLUTION (Choose One)

### **Option 1: Automated PowerShell Script (EASIEST)** ⭐

**On EC2 Server, run as Administrator:**
```powershell
# Download and run the fix script
cd C:\Websites\ET
.\Fix-AwsCompilationError.ps1
```

This script will:
1. ✅ Remove AWS folder if it exists
2. ✅ Stop IIS
3. ✅ Clear ASP.NET temporary compiled files
4. ✅ Start IIS
5. ✅ Force recompilation

---

### **Option 2: Manual Commands (2 MINUTES)**

**On EC2 Server:**
```powershell
# 1. Stop IIS
iisreset /stop

# 2. Remove AWS folder (if exists)
Remove-Item "C:\inetpub\wwwroot\App_Code\AWS" -Recurse -Force -ErrorAction SilentlyContinue

# 3. Clear compiled cache
Remove-Item "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Temporary ASP.NET Files\*" -Recurse -Force

# 4. Start IIS
iisreset /start

# 5. Touch web.config to force recompile
(Get-Item "C:\inetpub\wwwroot\web.config").LastWriteTime = Get-Date
```

---

### **Option 3: Simple IIS Restart (30 SECONDS)**

**Try this first - it might be enough:**
```powershell
iisreset
```

Then touch web.config:
```powershell
(Get-Item "C:\inetpub\wwwroot\web.config").LastWriteTime = Get-Date
```

---

## 🔍 Root Cause

**What happened:**
1. AWS helper files were created in `App_Code\AWS\`
2. ASP.NET compiled them into DLLs
3. These compiled DLLs reference AWS SDK (Amazon.*)
4. We deleted the source files but IIS cached the compiled DLLs
5. IIS tries to use cached DLLs → error because AWS SDK isn't installed

**Why it's happening:**
- ASP.NET Web Sites compile on-demand and cache results
- Deleting source files doesn't automatically clear cache
- Cache is in `C:\Windows\Microsoft.NET\...\Temporary ASP.NET Files`

---

## ✅ Verification

After running the fix, test:

1. **Browse to:** `https://yoursite.com/custlookuplnhp.aspx`
2. **Expected:** Page loads successfully (no AWS error)
3. **Check:** Calendar picker icon appears
4. **Test:** Edit a customer, set appointment, verify email sent

---

## 📁 File Structure (After Fix)

**These files should exist:**
```
✅ custlookuplnhp.aspx
✅ custlookuplnhp.aspx.cs
✅ App_Code\EmailHelper.cs
✅ App_Code\Helpers\TimezoneHelper.cs
✅ App_Code\Models\Customer.cs
✅ App_Code\Repositories\CustomerRepository.cs
✅ App_Code\Services\AgentEmailService.cs
```

**These files should NOT exist:**
```
❌ App_Code\AWS\AwsSecretsHelper.cs
❌ App_Code\AWS\AwsSesEmailService.cs
❌ App_Code\AWS\CloudWatchLogger.cs
```

**Check on server:**
```powershell
# Should return nothing:
Get-ChildItem "C:\inetpub\wwwroot\App_Code\AWS" -ErrorAction SilentlyContinue

# Should return 5 files:
Get-ChildItem "C:\inetpub\wwwroot\App_Code" -Recurse -Filter "*.cs"
```

---

## 🚨 If Problem Persists

### 1. Verify AWS folder is gone
```powershell
Test-Path "C:\inetpub\wwwroot\App_Code\AWS"
# Should return: False
```

If it returns `True`, manually delete:
```powershell
Remove-Item "C:\inetpub\wwwroot\App_Code\AWS" -Recurse -Force
```

### 2. Check for deployment issues
```powershell
# Make sure you're looking at the right website folder
Get-ChildItem "C:\inetpub\wwwroot" | Select-Object Name
```

### 3. Clear browser cache
Sometimes browsers cache error pages:
```
Ctrl + Shift + Delete → Clear cached images and files
```

### 4. Check IIS Application Pool
```powershell
Import-Module WebAdministration
Get-WebAppPoolState -Name "DefaultAppPool"  # Should be "Started"
```

If stopped, start it:
```powershell
Start-WebAppPool -Name "DefaultAppPool"
```

### 5. Check Event Viewer
```
Windows Event Viewer → Application logs
Look for ASP.NET errors around the time you accessed the page
```

---

## 📝 Understanding ASP.NET Compilation

### How ASP.NET Web Sites Work:
```
User visits page
     ↓
ASP.NET checks if page is compiled
     ↓
If not compiled: Compile all App_Code files
     ↓
Store compiled DLLs in Temporary ASP.NET Files
     ↓
Use compiled DLLs to serve request
     ↓
Cache DLLs for future requests
```

### Why Clearing Cache Fixes It:
```
Old state: Cached DLLs reference AWS SDK
              ↓
Clear cache: Delete all cached DLLs
              ↓
Next request: ASP.NET recompiles from source
              ↓
New state: New DLLs without AWS references ✅
```

---

## 🎯 Quick Reference Commands

### Clear Cache (Most Reliable)
```powershell
iisreset /stop
Remove-Item "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Temporary ASP.NET Files\*" -Recurse -Force
iisreset /start
```

### Force Recompile (Fastest)
```powershell
(Get-Item "C:\inetpub\wwwroot\web.config").LastWriteTime = Get-Date
```

### Check App_Code Contents
```powershell
Get-ChildItem "C:\inetpub\wwwroot\App_Code" -Recurse -Filter "*.cs" | Select-Object Name
```

### Restart IIS (Simple)
```powershell
iisreset
```

---

## 📊 Expected Outcome

### Before Fix:
```
❌ CS0246: The type or namespace name 'Amazon' could not be found
❌ Page won't load
❌ Yellow error screen
```

### After Fix:
```
✅ Page loads successfully
✅ Calendar picker appears
✅ Can edit customers
✅ Timezone conversion works
✅ Emails sent with correct times
```

---

## 🔄 Alternative: Fresh Deployment

If all else fails, do a clean deployment:

```powershell
# 1. Stop IIS
iisreset /stop

# 2. Backup current site
Copy-Item "C:\inetpub\wwwroot" "C:\inetpub\wwwroot_backup_$(Get-Date -Format 'yyyyMMdd_HHmmss')" -Recurse

# 3. Delete App_Code completely
Remove-Item "C:\inetpub\wwwroot\App_Code" -Recurse -Force

# 4. Copy fresh App_Code from dev machine (without AWS folder)

# 5. Clear cache
Remove-Item "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Temporary ASP.NET Files\*" -Recurse -Force

# 6. Start IIS
iisreset /start
```

---

## ✅ Success Checklist

After applying the fix:

- [ ] Run one of the fix options above
- [ ] Browse to `custlookuplnhp.aspx`
- [ ] Page loads without errors
- [ ] Calendar icon appears
- [ ] Click Edit on a customer
- [ ] Calendar picker opens
- [ ] Set appointment time
- [ ] Click Update
- [ ] Email sent successfully
- [ ] Calendar invite shows correct time

**All checkboxes ✅ = SUCCESS!** 🎉

---

## 🆘 Need Help?

If none of the above works:

1. Check `FIX_AWS_COMPILATION_ERROR.md` for detailed troubleshooting
2. Run `Fix-AwsCompilationError.ps1` script
3. Check Windows Event Viewer for specific errors
4. Verify IIS application pool is running
5. Ensure .NET Framework 4.0 is installed

---

## 📞 Quick Support

**Error:** Amazon namespace not found  
**Cause:** Cached compiled files reference AWS SDK  
**Fix:** Clear ASP.NET temporary files and restart IIS  
**Time:** 2-5 minutes  
**Risk:** Zero - just clears cache  

**Run this command and you'll be fixed:**
```powershell
iisreset /stop; Remove-Item "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Temporary ASP.NET Files\*" -Recurse -Force; iisreset /start
```

---

## Status: ⚠️ REQUIRES SERVER-SIDE ACTION

The source code is already fixed. You just need to **clear the IIS cache on the server**. Choose one of the options above and you'll be running in 2 minutes!

🎯 **Recommended:** Use Option 3 (Simple IIS Restart) first. If that doesn't work, use Option 1 (PowerShell Script).
