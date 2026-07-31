using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

/// <summary>
/// Sends emails using AWS SES instead of third-party SMTP
/// Benefits:
/// - No password management (uses IAM role)
/// - Better deliverability
/// - Built-in bounce/complaint handling
/// - Cheaper ($0.10 per 1000 emails)
/// - Integrated CloudWatch metrics
/// 
/// Prerequisites:
/// - Verify sender email in SES Console
/// - Move out of SES Sandbox (submit support request)
/// - Attach IAM policy to EC2 role: ses:SendEmail, ses:SendRawEmail
/// </summary>
public static class AwsSesEmailService
{
    private static readonly string Region = "us-east-1"; // Change to your region
    private static readonly IAmazonSimpleEmailService _sesClient;

    static AwsSesEmailService()
    {
        // Uses IAM role - no credentials needed!
        _sesClient = new AmazonSimpleEmailServiceClient(RegionEndpoint.GetBySystemName(Region));
    }

    /// <summary>
    /// Sends appointment email with ICS calendar attachment using AWS SES
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

        try
        {
            // Build ICS calendar file
            var icsContent = BuildIcsContent(fromEmail, subject, body, appointmentDate, location, toEmails.FirstOrDefault());

            // Build MIME message with ICS attachment
            var rawMessage = BuildRawEmailMessage(
                fromEmail, fromDisplayName,
                toEmails, toDisplayNames,
                ccEmails, ccDisplayNames,
                subject, body, icsContent);

            // Send via SES
            var request = new SendRawEmailRequest
            {
                RawMessage = new RawMessage
                {
                    Data = new MemoryStream(Encoding.UTF8.GetBytes(rawMessage))
                }
            };

            var response = _sesClient.SendRawEmailAsync(request).Result;

            // Success - SES returns MessageId
            System.Diagnostics.Debug.WriteLine(string.Format("Email sent successfully. SES MessageId: {0}", response.MessageId));
            return true;
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
            System.Diagnostics.Debug.WriteLine(string.Format("Failed to send email via SES: {0}", ex));
            return false;
        }
    }

    private static string BuildIcsContent(string organizerEmail, string subject, string body, DateTime appointmentDate, string location, string attendeeEmail)
    {
        var sb = new StringBuilder();
        sb.AppendLine("BEGIN:VCALENDAR");
        sb.AppendLine("PRODID:-//Asher Solutions//Appointment Scheduler//EN");
        sb.AppendLine("VERSION:2.0");
        sb.AppendLine("METHOD:REQUEST");
        sb.AppendLine("BEGIN:VEVENT");
        // appointmentDate is already UTC from TimezoneHelper, just format it
        sb.AppendLine("DTSTART:" + appointmentDate.ToString("yyyyMMdd\\THHmmss\\Z"));
        sb.AppendLine("DTSTAMP:" + DateTime.UtcNow.ToString("yyyyMMdd\\THHmmss\\Z"));
        sb.AppendLine("DTEND:" + appointmentDate.AddHours(3).ToString("yyyyMMdd\\THHmmss\\Z"));
        sb.AppendLine("LOCATION:" + location);
        sb.AppendLine("UID:" + Guid.NewGuid().ToString());
        sb.AppendLine("DESCRIPTION:" + body.Replace("\n", "\\n"));
        sb.AppendLine("SUMMARY:" + subject);
        sb.AppendLine("ORGANIZER:MAILTO:" + organizerEmail);

        if (!string.IsNullOrEmpty(attendeeEmail))
        {
            sb.AppendLine(string.Format("ATTENDEE;ROLE=REQ-PARTICIPANT;RSVP=TRUE:mailto:{0}", attendeeEmail));
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

    private static string BuildRawEmailMessage(
        string fromEmail, string fromDisplayName,
        IList<string> toEmails, IList<string> toDisplayNames,
        IList<string> ccEmails, IList<string> ccDisplayNames,
        string subject, string body, string icsContent)
    {
        var boundary = "----=_Part_" + Guid.NewGuid().ToString("N");
        var sb = new StringBuilder();

        // Email headers
        sb.AppendLine(string.Format("From: \"{0}\" <{1}>", fromDisplayName, fromEmail));

        // To recipients
        if (toEmails != null && toEmails.Count > 0)
        {
            var toList = string.Join(", ", toEmails.Select((email, i) =>
            {
                var name = toDisplayNames != null && toDisplayNames.Count > i ? toDisplayNames[i] : null;
                return string.IsNullOrEmpty(name) ? email : string.Format("\"{0}\" <{1}>", name, email);
            }));
            sb.AppendLine(string.Format("To: {0}", toList));
        }

        // CC recipients
        if (ccEmails != null && ccEmails.Count > 0)
        {
            var ccList = string.Join(", ", ccEmails.Select((email, i) =>
            {
                var name = ccDisplayNames != null && ccDisplayNames.Count > i ? ccDisplayNames[i] : null;
                return string.IsNullOrEmpty(name) ? email : string.Format("\"{0}\" <{1}>", name, email);
            }));
            sb.AppendLine(string.Format("Cc: {0}", ccList));
        }

        sb.AppendLine(string.Format("Subject: {0}", subject));
        sb.AppendLine("MIME-Version: 1.0");
        sb.AppendLine(string.Format("Content-Type: multipart/mixed; boundary=\"{0}\"", boundary));
        sb.AppendLine();

        // Email body
        sb.AppendLine(string.Format("--{0}", boundary));
        sb.AppendLine("Content-Type: text/plain; charset=UTF-8");
        sb.AppendLine("Content-Transfer-Encoding: 7bit");
        sb.AppendLine();
        sb.AppendLine(body);
        sb.AppendLine();

        // ICS attachment
        sb.AppendLine(string.Format("--{0}", boundary));
        sb.AppendLine("Content-Type: text/calendar; charset=UTF-8; method=REQUEST; name=\"appointment.ics\"");
        sb.AppendLine("Content-Transfer-Encoding: base64");
        sb.AppendLine("Content-Disposition: attachment; filename=\"appointment.ics\"");
        sb.AppendLine();
        sb.AppendLine(Convert.ToBase64String(Encoding.UTF8.GetBytes(icsContent)));
        sb.AppendLine();

        // End boundary
        sb.AppendLine(string.Format("--{0}--", boundary));

        return sb.ToString();
    }

    /// <summary>
    /// Simple email send without attachments (faster, simpler API)
    /// </summary>
    public static bool SendSimpleEmail(string fromEmail, string toEmail, string subject, string body, out string errorMessage)
    {
        errorMessage = null;

        try
        {
            var request = new SendEmailRequest
            {
                Source = fromEmail,
                Destination = new Destination
                {
                    ToAddresses = new List<string> { toEmail }
                },
                Message = new Message
                {
                    Subject = new Content(subject),
                    Body = new Body
                    {
                        Text = new Content(body)
                    }
                }
            };

            var response = _sesClient.SendEmailAsync(request).Result;
            return true;
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
            return false;
        }
    }
}
