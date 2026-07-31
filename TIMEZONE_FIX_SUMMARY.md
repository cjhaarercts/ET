# Timezone Fix - Complete Summary

## ✅ Problem Solved

**Issue:** Calendar appointment invites showed wrong times because:
- AWS server runs in UTC timezone
- Users enter times in Eastern Time
- Code incorrectly converted Eastern → UTC

**Result:** Appointments appeared 4-5 hours off in user's calendar

---

## ✅ Solution Implemented

### Files Changed:

1. **`custlookuplnhp.aspx`**
   - Added hidden field `hdnTimezoneOffset` to capture browser timezone
   - Added JavaScript to detect user's timezone offset automatically

2. **`custlookuplnhp.aspx.cs`**
   - Updated `btnUpdate_Click` to properly convert user time → UTC
   - Updated `SendEmail` method signature to pass both UTC and display times
   - Added proper timezone handling with fallback to Eastern Time

3. **`App_Code/EmailHelper.cs`**
   - Removed `.ToUniversalTime()` double-conversion bug
   - Now expects UTC time directly from calling code

4. **`App_Code/AWS/AwsSesEmailService.cs`**
   - Same fix as EmailHelper for AWS SES version

5. **`App_Code/Helpers/TimezoneHelper.cs`** (NEW)
   - Central timezone conversion logic
   - Handles browser offset → UTC conversion
   - Fallback to Eastern Time if no browser info

---

## 🔄 How It Works Now

```
┌──────────────────┐
│ User's Browser   │  
│ (Eastern Time)   │  User enters: "2:00 PM" on Jan 15, 2025
└─────────┬────────┘  
          │           JavaScript captures: timezoneOffset = -300 (EST = UTC-5)
          ▼
┌──────────────────┐
│ AWS EC2 Server   │
│ (UTC timezone)   │  Receives: "2:00 PM" + offset "-300"
└─────────┬────────┘  Converts: 2:00 PM + 300 min = 7:00 PM UTC
          │           Saves to DB: 7:00 PM UTC
          ▼
┌──────────────────┐
│ Email with ICS   │  ICS file: DTSTART:20250115T190000Z (7 PM UTC)
└─────────┬────────┘  
          │
          ▼
┌──────────────────┐
│ User's Calendar  │  Shows: 2:00 PM EST ✅ (automatic conversion)
│ (Outlook/Google) │
└──────────────────┘
```

---

## 🧪 Testing Required

### Before Deploying to Production:

1. **Test with Eastern Time user** (most common)
   ```
   - Enter: 2:00 PM
   - Expect calendar to show: 2:00 PM EST/EDT
   ```

2. **Test with Pacific Time user**
   ```
   - Change Windows timezone to Pacific
   - Enter: 2:00 PM
   - Expect calendar to show: 2:00 PM PST/PDT
   ```

3. **Test fallback (disable JavaScript)**
   ```
   - Block JavaScript in browser
   - Enter: 2:00 PM
   - Should default to Eastern Time
   ```

4. **Test in multiple calendar apps**
   - Outlook desktop
   - Outlook web
   - Google Calendar
   - iPhone Calendar
   - Android Calendar

5. **Verify ICS file format**
   ```
   - Save .ics attachment
   - Open in text editor
   - Verify: DTSTART:20250115T190000Z (ends with Z for UTC)
   ```

---

## 📝 Deployment Steps

### 1. Backup Current Code
```powershell
# On EC2 server
Copy-Item C:\inetpub\wwwroot\custlookuplnhp.aspx C:\backups\custlookuplnhp.aspx.bak
Copy-Item C:\inetpub\wwwroot\custlookuplnhp.aspx.cs C:\backups\custlookuplnhp.aspx.cs.bak
Copy-Item C:\inetpub\wwwroot\App_Code\EmailHelper.cs C:\backups\EmailHelper.cs.bak
```

### 2. Deploy New Files
```powershell
# Copy all updated files to server
# - custlookuplnhp.aspx
# - custlookuplnhp.aspx.cs
# - App_Code/EmailHelper.cs
# - App_Code/AWS/AwsSesEmailService.cs
# - App_Code/Helpers/TimezoneHelper.cs (new)
```

### 3. Test on Server
```
1. Browse to: https://yoursite.com/custlookuplnhp.aspx
2. Open browser console (F12)
3. Check for JavaScript errors
4. Create test appointment
5. Verify email sent
6. Check calendar invite shows correct time
```

### 4. Monitor CloudWatch Logs
```
- Go to CloudWatch Console
- Check /aws/ec2/veteransprogram logs
- Look for any timezone-related errors
```

### 5. Rollback if Needed
```powershell
# If issues occur, restore backups
Copy-Item C:\backups\*.bak C:\inetpub\wwwroot\ -Force
iisreset
```

---

## 🚀 Apply to Other Pages

This fix is currently only on `custlookuplnhp.aspx`.

**Other pages that need the same fix:**
- [ ] `TestCustomer.aspx`
- [ ] `Customer.aspx`
- [ ] `cluAgentCBTickler.aspx`
- [ ] `cluAgentLMTickler.aspx`
- [ ] Any other page with `obout:Calendar` control

**To apply the fix to another page:**

1. Add hidden field in .aspx:
   ```aspx
   <asp:HiddenField ID="hdnTimezoneOffset" runat="server" />
   ```

2. Add JavaScript in .aspx:
   ```javascript
   window.onload = function() {
       var offset = new Date().getTimezoneOffset();
       var field = document.getElementById('<%= hdnTimezoneOffset.ClientID %>');
       if (field) field.value = offset;
   };
   ```

3. Update .aspx.cs code-behind:
   ```csharp
   int timezoneOffset = 0;
   if (!string.IsNullOrEmpty(hdnTimezoneOffset.Value))
       int.TryParse(hdnTimezoneOffset.Value, out timezoneOffset);

   DateTime utc = timezoneOffset != 0 
       ? TimezoneHelper.ConvertToUtcForIcs(parsedDate, timezoneOffset)
       : TimezoneHelper.ConvertEasternToUtcForIcs(parsedDate);
   ```

---

## 💡 Key Takeaways

1. **AWS servers run in UTC** - always check server timezone
2. **User input is ambiguous** - "2:00 PM" could be any timezone
3. **Capture browser timezone** - JavaScript can detect it automatically
4. **Store UTC in database** - convert to user's timezone only for display
5. **ICS files use UTC** - calendar apps handle conversion automatically
6. **Always test timezones** - EST/EDT, PST/PDT, across DST boundaries

---

## 📚 Documentation Files

- **`docs/TIMEZONE_FIX.md`** - Detailed technical explanation
- **`App_Code/Helpers/TimezoneHelper.cs`** - Reusable conversion logic
- **`AWS_MODERNIZATION_ROADMAP.md`** - Full AWS optimization guide
- **`MODERNIZATION_GUIDE.md`** - General modernization strategy

---

## ✅ Checklist for Production

- [x] Timezone conversion logic implemented
- [x] JavaScript captures browser offset
- [x] Fallback to Eastern Time if no JS
- [x] EmailHelper updated (no double conversion)
- [x] AWS SES version updated
- [x] Documentation created
- [ ] **Test with real Eastern Time user**
- [ ] **Test with real Pacific Time user**
- [ ] **Test in Outlook**
- [ ] **Test in Google Calendar**
- [ ] **Test on mobile devices**
- [ ] **Deploy to production**
- [ ] **Monitor CloudWatch logs for 24 hours**
- [ ] **Apply fix to other pages**

---

**Status:** Ready for testing ✅

**Next Step:** Deploy to staging/test environment and verify calendar times are correct
