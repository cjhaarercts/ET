using System;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

/// <summary>
/// Retrieves secrets from AWS Secrets Manager instead of hardcoding in source code
/// Requires: AWSSDK.SecretsManager NuGet package
/// IAM Role: EC2 instance needs secretsmanager:GetSecretValue permission
/// </summary>
public static class AwsSecretsHelper
{
    private static readonly string Region = "us-east-1";
    private static IAmazonSecretsManager _client;

    static AwsSecretsHelper()
    {
        _client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(Region));
    }

    public static string GetSecret(string secretName)
    {
        try
        {
            var request = new GetSecretValueRequest
            {
                SecretId = secretName
            };

            var response = _client.GetSecretValueAsync(request).Result;

            if (response.SecretString != null)
            {
                return response.SecretString;
            }
            else
            {
                var memoryStream = response.SecretBinary;
                var reader = new System.IO.StreamReader(memoryStream);
                return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Error retrieving secret: {0}", ex.Message));
            throw new InvalidOperationException(string.Format("Failed to retrieve secret from AWS Secrets Manager"), ex);
        }
    }

    public static SmtpCredentials GetSmtpCredentials(string secretName)
    {
        if (string.IsNullOrEmpty(secretName))
        {
            secretName = "prod/smtp/credentials";
        }

        var secretJson = GetSecret(secretName);
        var credentials = Newtonsoft.Json.JsonConvert.DeserializeObject<SmtpCredentials>(secretJson);
        return credentials;
    }
}

public class SmtpCredentials
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
}
