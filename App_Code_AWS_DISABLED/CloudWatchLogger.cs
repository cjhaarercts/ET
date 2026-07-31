using System;
using System.Collections.Generic;
using System.Diagnostics;
using Amazon;
using Amazon.CloudWatchLogs;
using Amazon.CloudWatchLogs.Model;

/// <summary>
/// Sends application logs to AWS CloudWatch Logs for centralized monitoring
/// Benefits:
/// - Centralized logging across all EC2 instances
/// - Query logs with CloudWatch Insights
/// - Set alarms on error rates
/// - Retain logs long-term
/// - No disk space issues on EC2
/// 
/// IAM Permissions needed:
/// - logs:CreateLogGroup
/// - logs:CreateLogStream
/// - logs:PutLogEvents
/// </summary>
public static class CloudWatchLogger
{
    private static readonly string Region = "us-east-1";
    private static readonly string LogGroupName = "/aws/ec2/veteransprogram";
    private static readonly string LogStreamName = string.Format("{0}-{1:yyyyMMdd}", Environment.MachineName, DateTime.UtcNow);
    private static readonly IAmazonCloudWatchLogs _cloudWatchClient;
    private static string _sequenceToken;

    static CloudWatchLogger()
    {
        try
        {
            _cloudWatchClient = new AmazonCloudWatchLogsClient(RegionEndpoint.GetBySystemName(Region));
            EnsureLogStreamExists();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(string.Format("Failed to initialize CloudWatch logger: {0}", ex.Message));
        }
    }

    private static void EnsureLogStreamExists()
    {
        try
        {
            // Create log group if doesn't exist
            try
            {
                _cloudWatchClient.CreateLogGroupAsync(new CreateLogGroupRequest
                {
                    LogGroupName = LogGroupName
                }).Wait();
            }
            catch
            {
                // Likely already exists, ignore
            }

            // Create log stream for this server/day
            try
            {
                _cloudWatchClient.CreateLogStreamAsync(new CreateLogStreamRequest
                {
                    LogGroupName = LogGroupName,
                    LogStreamName = LogStreamName
                }).Wait();
            }
            catch
            {
                // Likely already exists, ignore
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(string.Format("Error ensuring log stream exists: {0}", ex.Message));
        }
    }

    public static void LogInfo(string message, Dictionary<string, string> properties = null)
    {
        Log("INFO", message, properties);
    }

    public static void LogWarning(string message, Dictionary<string, string> properties = null)
    {
        Log("WARNING", message, properties);
    }

    public static void LogError(string message, Exception exception = null, Dictionary<string, string> properties = null)
    {
        var props = properties ?? new Dictionary<string, string>();
        if (exception != null)
        {
            props["ExceptionType"] = exception.GetType().Name;
            props["ExceptionMessage"] = exception.Message;
            props["StackTrace"] = exception.StackTrace;
        }
        Log("ERROR", message, props);
    }

    private static void Log(string level, string message, Dictionary<string, string> properties)
    {
        try
        {
            // Build structured log message
            var logMessage = string.Format("[{0}] {1}", level, message);
            if (properties != null && properties.Count > 0)
            {
                var propsJson = Newtonsoft.Json.JsonConvert.SerializeObject(properties);
                logMessage += " " + propsJson;
            }

            // Also log locally for debugging
            Debug.WriteLine(logMessage);

            // Send to CloudWatch
            if (_cloudWatchClient != null)
            {
                var request = new PutLogEventsRequest
                {
                    LogGroupName = LogGroupName,
                    LogStreamName = LogStreamName,
                    LogEvents = new List<InputLogEvent>
                    {
                        new InputLogEvent
                        {
                            Timestamp = DateTime.UtcNow,
                            Message = logMessage
                        }
                    }
                };

                if (!string.IsNullOrEmpty(_sequenceToken))
                {
                    request.SequenceToken = _sequenceToken;
                }

                var response = _cloudWatchClient.PutLogEventsAsync(request).Result;
                _sequenceToken = response.NextSequenceToken;
            }
        }
        catch (Exception ex)
        {
            // Don't crash app if logging fails
            Debug.WriteLine(string.Format("Failed to send log to CloudWatch: {0}", ex.Message));
        }
    }

    /// <summary>
    /// Log customer update operation
    /// </summary>
    public static void LogCustomerUpdate(int customerId, string userName, bool success, string errorMessage)
    {
        var properties = new Dictionary<string, string>();
        properties.Add("CustomerId", customerId.ToString());
        properties.Add("UserName", userName ?? "Anonymous");
        properties.Add("Success", success.ToString());
        properties.Add("Page", "custlookuplnhp.aspx");

        if (!string.IsNullOrEmpty(errorMessage))
        {
            properties.Add("ErrorMessage", errorMessage);
            LogError(string.Format("Customer update failed for ID {0}", customerId), null, properties);
        }
        else
        {
            LogInfo(string.Format("Customer {0} updated successfully", customerId), properties);
        }
    }

    /// <summary>
    /// Log email send operation
    /// </summary>
    public static void LogEmailSent(string recipientEmail, string subject, bool success, string errorMessage)
    {
        var properties = new Dictionary<string, string>();
        properties.Add("RecipientEmail", recipientEmail);
        properties.Add("Subject", subject);
        properties.Add("Success", success.ToString());

        if (!string.IsNullOrEmpty(errorMessage))
        {
            properties.Add("ErrorMessage", errorMessage);
            LogError(string.Format("Failed to send email to {0}", recipientEmail), null, properties);
        }
        else
        {
            LogInfo(string.Format("Email sent successfully to {0}", recipientEmail), properties);
        }
    }
}
