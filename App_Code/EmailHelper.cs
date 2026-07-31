using System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Text;

public static class EmailHelper
{
    /// <summary>
    /// Sends an appointment email with an ICS calendar attachment using the shared
    /// Asher SMTP configuration. Returns true on success; on failure, returns false
    /// and populates errorMessage with the exception message.
    /// </summary>
    public static bool SendAppointmentEmailWithIcs(
        string fromEmail,
        string fromDisplayName,
        string subject,
        string body,
        DateTime appointmentDate,
        string location,
        IList<string> toEmails,
        IList<string> toDisplayNames,
        IList<string> ccEmails,
        IList<string> ccDisplayNames,
        out string errorMessage)
    {
        errorMessage = null;

        using (MailMessage msg = new MailMessage())
        {
            msg.From = new MailAddress(fromEmail, fromDisplayName);
            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = false;

            // add primary recipients
            if (toEmails != null)
            {
                for (int i = 0; i < toEmails.Count; i++)
                {
                    string email = toEmails[i];
                    if (string.IsNullOrEmpty(email))
                    {
                        continue;
                    }

                    string displayName = null;
                    if (toDisplayNames != null && toDisplayNames.Count > i)
                    {
                        displayName = toDisplayNames[i];
                    }

                    if (!string.IsNullOrEmpty(displayName))
                    {
                        msg.To.Add(new MailAddress(email, displayName));
                    }
                    else
                    {
                        msg.To.Add(new MailAddress(email));
                    }
                }
            }

            // add CC recipients if any
            if (ccEmails != null)
            {
                for (int i = 0; i < ccEmails.Count; i++)
                {
                    string email = ccEmails[i];
                    if (string.IsNullOrEmpty(email))
                    {
                        continue;
                    }

                    string displayName = null;
                    if (ccDisplayNames != null && ccDisplayNames.Count > i)
                    {
                        displayName = ccDisplayNames[i];
                    }

                    if (!string.IsNullOrEmpty(displayName))
                    {
                        msg.CC.Add(new MailAddress(email, displayName));
                    }
                    else
                    {
                        msg.CC.Add(new MailAddress(email));
                    }
                }
            }

            // build ICS content - appointmentDate is already in UTC from TimezoneHelper
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("PRODID:-//Schedule a Meeting");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("METHOD:REQUEST");
            sb.AppendLine("BEGIN:VEVENT");
            // appointmentDate is already UTC, just format it (don't call ToUniversalTime again)
            sb.AppendLine("DTSTART:" + appointmentDate.ToString("yyyyMMdd\\THHmmss\\Z"));
            sb.AppendLine("DTSTAMP:" + DateTime.UtcNow.ToString("yyyyMMdd\\THHmmss\\Z"));
            sb.AppendLine("DTEND:" + appointmentDate.AddMinutes(180).ToString("yyyyMMdd\\THHmmss\\Z"));
            sb.AppendLine("LOCATION: " + location);
            sb.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));
            sb.AppendLine(string.Format("DESCRIPTION:{0}", body));
            sb.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", body));
            sb.AppendLine(string.Format("SUMMARY:{0}", subject));
            sb.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", msg.From.Address));
            if (msg.To.Count > 0)
            {
                sb.AppendLine(string.Format("ATTENDEE;ROLE=OWNER;CN=\"{0}\";RSVP=TRUE:mailto:{1}", msg.To[0].DisplayName, msg.To[0].Address));
            }
            sb.AppendLine("BEGIN:VALARM");
            sb.AppendLine("TRIGGER:-PT30M");
            sb.AppendLine("ACTION:DISPLAY");
            sb.AppendLine("DESCRIPTION:Reminder");
            sb.AppendLine("END:VALARM");
            sb.AppendLine("END:VEVENT");
            sb.AppendLine("END:VCALENDAR");

            byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
            using (MemoryStream stream = new MemoryStream(bytes))
            using (Attachment attachment = new Attachment(stream, "appointment.ics", "text/calendar"))
            {
                msg.Attachments.Add(attachment);

                RemoteCertificateValidationCallback previousCallback = ServicePointManager.ServerCertificateValidationCallback;
                try
                {
                    try
                    {
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; // TLS 1.2
                    }
                    catch
                    {
                        // ignored for older frameworks
                    }

                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                    using (SmtpClient smtpClient = new SmtpClient("smtp.ashersolutions.com", 587))
                    {
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential("info@ashersolutions.com", "Fr3343v3r&^%");
                        smtpClient.EnableSsl = true;

                        try
                        {
                            smtpClient.Send(msg);
                            return true;
                        }
                        catch (Exception ex)
                        {
                            errorMessage = ex.Message;
                            return false;
                        }
                    }
                }
                finally
                {
                    ServicePointManager.ServerCertificateValidationCallback = previousCallback;
                }
            }
        }
    }

    /// <summary>
    /// Builds an ICS payload for the given appointment and returns it as a string.
    /// This overload is useful for legacy pages that already manage their own
    /// MailMessage and SmtpClient configuration but want a centralized ICS format.
    /// </summary>
    public static string BuildIcsContent(
        DateTime appointmentDate,
        string location,
        string body,
        string subject,
        string organizerEmail,
        string attendeeDisplayName,
        string attendeeEmail)
    {
        var sb = new StringBuilder();
        sb.AppendLine("BEGIN:VCALENDAR");
        sb.AppendLine("PRODID:-//Schedule a Meeting");
        sb.AppendLine("VERSION:2.0");
        sb.AppendLine("METHOD:REQUEST");
        sb.AppendLine("BEGIN:VEVENT");
        sb.AppendLine("DTSTART:" + appointmentDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"));
        sb.AppendLine("DTSTAMP:" + DateTime.UtcNow.ToString("yyyyMMdd\\THHmmss\\Z"));
        sb.AppendLine("DTEND:" + appointmentDate.AddMinutes(180).ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"));
        sb.AppendLine("LOCATION: " + location);
        sb.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));
        sb.AppendLine(string.Format("DESCRIPTION:{0}", body));
        sb.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", body));
        sb.AppendLine(string.Format("SUMMARY:{0}", subject));
        sb.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", organizerEmail));
        if (!string.IsNullOrEmpty(attendeeEmail))
        {
            sb.AppendLine(string.Format("ATTENDEE;ROLE=OWNER;CN=\"{0}\";RSVP=TRUE:mailto:{1}", attendeeDisplayName ?? string.Empty, attendeeEmail));
        }
        sb.AppendLine("BEGIN:VALARM");
        sb.AppendLine("TRIGGER:-PT30M");
        sb.AppendLine("ACTION:DISPLAY");
        sb.AppendLine("DESCRIPTION:Reminder");
        sb.AppendLine("END:VALARM");
        sb.AppendLine("END:VEVENT");
        sb.AppendLine("END:VCALENDAR");

        return sb.ToString();
    }
}
