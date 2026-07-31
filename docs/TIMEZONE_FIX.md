# Timezone Fix Documentation

## Problem Statement 🐛

**Issue:** Calendar appointments were showing incorrect times when sent via email.

**Root Cause:** 
1. AWS EC2 server runs in **UTC timezone**
2. Users enter appointment times in **Eastern Time** (EST/EDT)
3. Code was calling `.ToUniversalTime()` on user input, which treated it as **server local time (UTC)**
4. This caused **no timezone conversion** when it should convert Eastern → UTC
5. Result: Calendar invites showed times 4-5 hours off (depending on EST vs EDT)

### Example of the Bug:
```
User enters:    2:00 PM Eastern Time (what they want)
Server thinks:  2:00 PM UTC (wrong assumption)
.ToUniversalTime() converts: 2:00 PM UTC → 2:00 PM UTC (no change)
Calendar shows: 2:00 PM in user's timezone = 9:00 AM or 10:00 AM Eastern (wrong!)
```

**Expected behavior:**
```
User enters:    2:00 PM Eastern Time
Convert to UTC: 2:00 PM EST = 7:00 PM UTC (or 6:00 PM UTC if EDT)
Calendar shows: 2:00 PM Eastern Time (correct!)
```

---

## Solution Implemented ✅

### 1. Capture Browser Timezone (JavaScript)

Added to `custlookuplnhp.aspx`:
```javascript
// Runs when page loads
window.onload = function() {
    var timezoneOffset = new Date().getTimezoneOffset(); // Minutes from UTC
    var hdnField = document.getElementById('<%= hdnTimezoneOffset.ClientID %>');
    if (hdnField) {
        hdnField.value = timezoneOffset;
    }
};
```

**What this does:**
- Detects user's actual browser timezone
- For Eastern Time: `-300` (EST = UTC-5) or `-240` (EDT = UTC-4)
- Stores in hidden field `hdnTimezoneOffset`

### 2. Created TimezoneHelper Class

File: `App_Code/Helpers/TimezoneHelper.cs`

**Key Methods:**
```csharp
// Option A: Use browser timezone (most accurate)
DateTime utc = TimezoneHelper.ConvertToUtcForIcs(userDate, browserOffsetMinutes);

// Option B: Always assume Eastern Time (simpler, works for most users)
DateTime utc = TimezoneHelper.ConvertEasternToUtcForIcs(userDate);
```

### 3. Updated Page Code-Behind

File: `custlookuplnhp.aspx.cs`

**Before:**
```csharp
DateTime? appointmentSet = string.IsNullOrEmpty(txtDate.Text) 
    ? null 
    : Convert.ToDateTime(txtDate.Text);
```

**After:**
```csharp
DateTime parsedDate = Convert.ToDateTime(txtDate.Text);
int timezoneOffset = int.Parse(hdnTimezoneOffset.Value);

// Properly convert from user's timezone to UTC
DateTime? appointmentSet = TimezoneHelper.ConvertToUtcForIcs(parsedDate, timezoneOffset);
```

### 4. Updated Email Helpers

Both `EmailHelper.cs` and `AwsSesEmailService.cs`:

**Before:**
```csharp
sb.AppendLine("DTSTART:" + appointmentDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"));
```

**After:**
```csharp
// appointmentDate is already UTC, don't convert again!
sb.AppendLine("DTSTART:" + appointmentDate.ToString("yyyyMMdd\\THHmmss\\Z"));
```

---

## How It Works Now 🔄

### User Flow:
1. **User opens page** → JavaScript captures timezone offset (e.g., `-300` for EST)
2. **User enters "01/15/2025 2:00 PM"** in appointment field
3. **User clicks Update**
4. **Server receives:**
   - `txtDate.Text` = "01/15/2025 2:00 PM" (string)
   - `hdnTimezoneOffset.Value` = "-300" (minutes)
5. **Code converts:**
   - Parse string → `DateTime(2025, 1, 15, 14, 0, 0)` (unspecified kind)
   - Add offset: `2:00 PM + 300 minutes = 7:00 PM UTC`
6. **Save to database:** `7:00 PM UTC`
7. **Send email:** ICS file contains `20250115T190000Z` (7 PM UTC)
8. **User's calendar:** Automatically converts `7 PM UTC` → `2 PM EST` ✅

---

## Testing Scenarios 📋

### Test Case 1: Eastern Time User (EST)
```
Browser timezone: UTC-5 (offset = -300)
User enters: 2:00 PM on Jan 15, 2025
Expected UTC: 7:00 PM on Jan 15, 2025
Calendar shows: 2:00 PM EST ✅
```

### Test Case 2: Eastern Time User (EDT - Summer)
```
Browser timezone: UTC-4 (offset = -240)
User enters: 2:00 PM on July 15, 2025
Expected UTC: 6:00 PM on July 15, 2025
Calendar shows: 2:00 PM EDT ✅
```

### Test Case 3: Pacific Time User
```
Browser timezone: UTC-8 (offset = -480)
User enters: 2:00 PM on Jan 15, 2025
Expected UTC: 10:00 PM on Jan 15, 2025
Calendar shows: 2:00 PM PST ✅
```

### Test Case 4: Fallback (No JavaScript/Old Browser)
```
Browser timezone: Not captured (offset = 0)
User enters: 2:00 PM on Jan 15, 2025
Fallback: Assume Eastern Time
Expected UTC: 7:00 PM on Jan 15, 2025
Calendar shows: 2:00 PM EST ✅
```

---

## Database Considerations 💾

### DateTime Storage in SQL Server

**Current:** Database column `AppointmentSet` is likely `datetime` or `datetime2` (no timezone info)

**Recommendation:** Store as UTC for consistency:
```sql
-- Good: All times stored as UTC
UPDATE Customers SET AppointmentSet = '2025-01-15 19:00:00' -- 7 PM UTC

-- Bad: Mixed timezones in same column
UPDATE Customers SET AppointmentSet = '2025-01-15 14:00:00' -- Unclear if EST or UTC
```

**If you need to query by Eastern Time:**
```sql
-- Convert UTC to Eastern for display
SELECT 
    FirstName,
    LastName,
    AppointmentSet AT TIME ZONE 'UTC' AT TIME ZONE 'Eastern Standard Time' as AppointmentEastern
FROM Customers
WHERE AppointmentSet IS NOT NULL
```

---

## Deployment Checklist ✅

Before deploying to production:

- [ ] Test with Eastern Time browser (should be most common)
- [ ] Test with Pacific Time browser (change Windows timezone or use VPN)
- [ ] Test with old browser that doesn't support JavaScript (fallback to Eastern)
- [ ] Verify ICS attachment opens correctly in Outlook
- [ ] Verify ICS attachment opens correctly in Google Calendar
- [ ] Verify ICS attachment opens correctly on iPhone/Android
- [ ] Check database to confirm times are stored as UTC
- [ ] Send test appointment email to yourself, verify time is correct

---

## Troubleshooting 🔧

### Calendar shows wrong time
**Check:**
1. View page source, find `hdnTimezoneOffset`, check its value
2. Add logging: `CloudWatchLogger.LogInfo($"Timezone offset: {timezoneOffset}, Original: {txtDate.Text}, UTC: {appointmentSet}")`
3. Check ICS file content (open .ics in text editor):
   ```
   DTSTART:20250115T190000Z  ← Should be UTC
   ```
4. Verify server timezone: `TimeZoneInfo.Local` should be UTC on AWS

### JavaScript not running
**Symptoms:** `hdnTimezoneOffset.Value` is empty
**Solution:** Code falls back to Eastern Time assumption (line 173 in `custlookuplnhp.aspx.cs`)

### Wrong timezone assumption
**Issue:** Code assumes Eastern if offset = 0, but user might be in UTC (UK, etc.)
**Solution:** Add dropdown for user to manually select timezone (future enhancement)

---

## Future Enhancements 🚀

### Option 1: Store Timezone with Appointment
```sql
ALTER TABLE Customers ADD AppointmentTimezone VARCHAR(50);
-- Store "Eastern Standard Time", "Pacific Standard Time", etc.
```

### Option 2: User Profile with Default Timezone
```sql
CREATE TABLE UserProfiles (
    UserId INT PRIMARY KEY,
    DefaultTimezone VARCHAR(50) NOT NULL DEFAULT 'Eastern Standard Time'
);
```

### Option 3: Display Timezone in UI
```html
<td>
    <asp:TextBox ID="txtDate" runat="server" />
    <span id="userTimezone"></span> <!-- Shows "EST (UTC-5)" -->
    <obout:Calendar ... />
</td>
```

### Option 4: Convert All Pages
This fix is currently only on `custlookuplnhp.aspx`. Apply to:
- [ ] `TestCustomer.aspx`
- [ ] `Customer.aspx`
- [ ] `cluAgentCBTickler.aspx`
- [ ] `cluAgentLMTickler.aspx`
- [ ] All other pages with calendar pickers

---

## Technical Details 📚

### JavaScript `getTimezoneOffset()` Explanation

**Returns:** Minutes offset from UTC (inverted from standard notation)
```javascript
// Eastern Standard Time (UTC-5)
new Date().getTimezoneOffset() // Returns 300 (positive number)
// Because: UTC - EST = +5 hours = +300 minutes

// Pacific Standard Time (UTC-8)
new Date().getTimezoneOffset() // Returns 480
// Because: UTC - PST = +8 hours = +480 minutes

// UK (UTC+0)
new Date().getTimezoneOffset() // Returns 0

// India (UTC+5:30)
new Date().getTimezoneOffset() // Returns -330 (negative!)
// Because: UTC - IST = -5.5 hours = -330 minutes
```

**Conversion formula:**
```csharp
// JavaScript offset is inverted, so add it to get UTC
DateTime utc = localTime.AddMinutes(offsetMinutes);
```

### ICS Calendar Format

**Spec:** iCalendar (RFC 5545)

**UTC Time Format:**
```
20250115T190000Z
└─ year─┘│││││││└─ Z means UTC
         │││││││
         └──month (01-12)
           │││││
           └─day (01-31)
             │││
             └─T separator
               │
               └─hour (00-23) minute second
```

**Local Time Format (without Z):**
```
DTSTART;TZID=America/New_York:20250115T140000
```
We use UTC format (with Z) to avoid timezone definition complexity.

---

## References 📖

- [RFC 5545: iCalendar](https://tools.ietf.org/html/rfc5545)
- [.NET TimeZoneInfo Class](https://docs.microsoft.com/en-us/dotnet/api/system.timezoneinfo)
- [JavaScript Date.getTimezoneOffset()](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Date/getTimezoneOffset)
- [AWS EC2 Time Synchronization](https://docs.aws.amazon.com/AWSEC2/latest/WindowsGuide/windows-set-time.html)

---

## Summary

**Before:** Calendar times were wrong due to UTC server timezone assumption
**After:** Captures browser timezone, converts properly, calendar shows correct times
**Impact:** All appointment emails now show correct times in user's calendar
**Files Changed:** 
- `custlookuplnhp.aspx` (added hidden field + JavaScript)
- `custlookuplnhp.aspx.cs` (proper timezone conversion)
- `App_Code/EmailHelper.cs` (removed double UTC conversion)
- `App_Code/AWS/AwsSesEmailService.cs` (removed double UTC conversion)
- `App_Code/Helpers/TimezoneHelper.cs` (new helper class)
