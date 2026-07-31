# AWS Secrets Manager Setup Instructions

## Step 1: Create Secret in AWS Console

1. Open AWS Secrets Manager: https://console.aws.amazon.com/secretsmanager/
2. Click "Store a new secret"
3. Select "Other type of secret"
4. Key/value pairs:
   ```
   username: info@ashersolutions.com
   password: Fr3343v3r&^%
   host: smtp.ashersolutions.com
   port: 587
   ```
5. Secret name: `prod/smtp/credentials`
6. Click "Next" → "Next" → "Store"

## Step 2: Grant EC2 IAM Role Access

### Option A: Using AWS Console
1. Go to IAM → Roles
2. Find your EC2 instance role (or create one)
3. Attach policy:
   ```json
   {
     "Version": "2012-10-17",
     "Statement": [
       {
         "Effect": "Allow",
         "Action": [
           "secretsmanager:GetSecretValue",
           "secretsmanager:DescribeSecret"
         ],
         "Resource": "arn:aws:secretsmanager:us-east-1:*:secret:prod/smtp/*"
       }
     ]
   }
   ```
4. Attach role to EC2 instance (EC2 → Actions → Security → Modify IAM role)

### Option B: Using AWS CLI
```bash
# Create IAM policy
aws iam create-policy \
  --policy-name EC2-SecretsManager-SMTP \
  --policy-document file://secrets-policy.json

# Attach to EC2 instance role
aws iam attach-role-policy \
  --role-name YOUR_EC2_ROLE_NAME \
  --policy-arn arn:aws:iam::YOUR_ACCOUNT_ID:policy/EC2-SecretsManager-SMTP
```

## Step 3: Install NuGet Package

Run in Visual Studio Package Manager Console:
```powershell
Install-Package AWSSDK.SecretsManager
```

Or add to `packages.config`:
```xml
<package id="AWSSDK.SecretsManager" version="3.7.300.0" targetFramework="net40" />
```

## Step 4: Update EmailHelper.cs

Replace hardcoded credentials:
```csharp
// OLD (INSECURE)
smtpClient.Credentials = new NetworkCredential("info@ashersolutions.com", "Fr3343v3r&^%");

// NEW (SECURE)
var smtpCreds = AwsSecretsHelper.GetSmtpCredentials();
smtpClient.Credentials = new NetworkCredential(smtpCreds.Username, smtpCreds.Password);
```

## Step 5: Test

1. RDP to EC2 instance
2. Open browser on custlookuplnhp.aspx
3. Update a customer with appointment date
4. Check email was sent successfully
5. Check CloudWatch Logs for any errors

## Cost
- Secrets Manager: $0.40/month per secret + $0.05 per 10,000 API calls
- Typical usage: ~$0.50/month total

## Security Benefits
✅ No passwords in source code
✅ Automatic rotation possible
✅ Audit trail (who accessed secret when)
✅ Fine-grained access control
✅ Encrypted at rest and in transit
