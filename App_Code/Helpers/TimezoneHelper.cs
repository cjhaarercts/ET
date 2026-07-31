using System;

/// <summary>
/// Handles timezone conversions for appointment scheduling
/// Problem: AWS server runs in UTC, but users enter times in their local timezone (usually Eastern Time)
/// Solution: Capture browser timezone offset and convert properly for ICS calendar files
/// </summary>
public static class TimezoneHelper
{
    // Eastern Time Zone (handles both EST and EDT automatically)
    private static readonly TimeZoneInfo EasternTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

    /// <summary>
    /// Converts a user-entered date/time to Eastern Time, then to UTC for ICS calendar
    /// </summary>
    /// <param name="userDateTime">The date/time string entered by the user (e.g., "01/15/2025 02:30 PM")</param>
    /// <param name="browserTimezoneOffsetMinutes">Browser's timezone offset in minutes from UTC (captured via JavaScript)</param>
    /// <returns>DateTime in UTC for use in ICS file</returns>
    public static DateTime ConvertToUtcForIcs(DateTime userDateTime, int browserTimezoneOffsetMinutes)
    {
        // Browser offset is in minutes from UTC (e.g., -300 for EST = UTC-5, -240 for EDT = UTC-4)
        // Negative means ahead of UTC, positive means behind UTC (JavaScript convention)

        // Step 1: Treat the user input as being in their browser's timezone
        // We need to add the offset to get to UTC
        DateTime utcDateTime = userDateTime.AddMinutes(browserTimezoneOffsetMinutes);

        return utcDateTime;
    }

    /// <summary>
    /// Converts a user-entered date/time to Eastern Time (the most common timezone for this app)
    /// Use this if you want to always interpret user input as Eastern Time regardless of browser
    /// </summary>
    /// <param name="userDateTime">The date/time entered by user (assumed to be Eastern Time)</param>
    /// <returns>DateTime in UTC for use in ICS file</returns>
    public static DateTime ConvertEasternToUtcForIcs(DateTime userDateTime)
    {
        // Specify that the user's date is in Eastern Time (unspecified kind needs to be marked)
        DateTime easternDateTime = DateTime.SpecifyKind(userDateTime, DateTimeKind.Unspecified);

        // Convert Eastern Time to UTC
        DateTime utcDateTime = TimeZoneInfo.ConvertTimeToUtc(easternDateTime, EasternTimeZone);

        return utcDateTime;
    }

    /// <summary>
    /// Formats a UTC DateTime for ICS calendar files
    /// </summary>
    public static string FormatForIcs(DateTime utcDateTime)
    {
        return utcDateTime.ToString("yyyyMMdd\\THHmmss\\Z");
    }

    /// <summary>
    /// Converts UTC time back to Eastern Time for display
    /// </summary>
    public static DateTime ConvertUtcToEastern(DateTime utcDateTime)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, EasternTimeZone);
    }

    /// <summary>
    /// Gets a user-friendly timezone name based on offset
    /// </summary>
    public static string GetTimezoneNameFromOffset(int offsetMinutes)
    {
        // Common US timezones
        switch (offsetMinutes)
        {
            case 300: return "EST (UTC-5)"; // Eastern Standard Time
            case 240: return "EDT (UTC-4)"; // Eastern Daylight Time
            case 360: return "CST (UTC-6)"; // Central Standard Time
            case 300: return "CDT (UTC-5)"; // Central Daylight Time
            case 420: return "MST (UTC-7)"; // Mountain Standard Time
            case 360: return "MDT (UTC-6)"; // Mountain Daylight Time
            case 480: return "PST (UTC-8)"; // Pacific Standard Time
            case 420: return "PDT (UTC-7)"; // Pacific Daylight Time
            case 0: return "UTC";
            default: return string.Format("UTC{0}{1}", (offsetMinutes > 0 ? "+" : ""), -offsetMinutes / 60);
        }
    }
}
