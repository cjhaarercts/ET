# AWS-Specific Modernization Roadmap

## 🎯 Key Insight: You're Already on Modern Infrastructure!

**Current:** Windows Server 2022 + AWS RDS SQL = Can run .NET 8 **today**

You have a **huge advantage** - no need to migrate infrastructure. Your EC2 instance already supports the latest .NET runtime.

---

## Quick Comparison: What You're Missing

| Service | Current | AWS-Optimized | Benefit |
|---------|---------|---------------|---------|
| **Secrets** | Hardcoded passwords | AWS Secrets Manager | Secure, auditable, rotatable |
| **Email** | Third-party SMTP ($?) | AWS SES ($0.10/1000) | Cheaper, better deliverability, no password |
| **Logging** | None/local files | CloudWatch Logs | Centralized, searchable, alertable |
| **Monitoring** | Manual | CloudWatch Metrics | Automatic dashboards & alarms |
| **Scaling** | Single EC2 | Auto Scaling Group + ALB | High availability, load balancing |
| **Background Jobs** | Blocking sync | SQS + Lambda | Non-blocking, scalable |
| **Deployment** | Manual RDP | CodeDeploy | Automated, zero-downtime |

---

## Phase 1: Security & Observability (Week 1-2) 🔒

### Priority 1: Remove Hardcoded Password

**Current Risk:** SMTP password visible in `App_Code/EmailHelper.cs` line 147

**Action Steps:**
1. Install NuGet package:
   ```powershell
   Install-Package AWSSDK.SecretsManager -Version 3.7.300
   ```

2. Create secret in AWS Console:
   - Service: Secrets Manager
   - Secret type: Other
   - Name: `prod/smtp/credentials`
   - Value (JSON):
     ```json
     {
       "username": "info@ashersolutions.com",
       "password": "Fr3343v3r&^%",
       "host": "smtp.ashersolutions.com",
       "port": 587
     }
     ```

3. Attach IAM policy to EC2 role:
   ```json
   {
     "Effect": "Allow",
     "Action": "secretsmanager:GetSecretValue",
     "Resource": "arn:aws:secretsmanager:us-east-1:*:secret:prod/smtp/*"
   }
   ```

4. Update `EmailHelper.cs`:
   ```csharp
   // Replace line 147
   var creds = AwsSecretsHelper.GetSmtpCredentials();
   smtpClient.Credentials = new NetworkCredential(creds.Username, creds.Password);
   ```

**Cost:** ~$0.50/month
**Time:** 30 minutes
**Risk:** Very low

---

### Priority 2: Add CloudWatch Logging

**Current Problem:** No visibility when errors occur, no audit trail

**Action Steps:**
1. Install NuGet package:
   ```powershell
   Install-Package AWSSDK.CloudWatchLogs -Version 3.7.300
   ```

2. Attach IAM policy:
   ```json
   {
     "Effect": "Allow",
     "Action": [
       "logs:CreateLogGroup",
       "logs:CreateLogStream",
       "logs:PutLogEvents"
     ],
     "Resource": "arn:aws:logs:us-east-1:*:log-group:/aws/ec2/veteransprogram:*"
   }
   ```

3. Code already updated in `custlookuplnhp.aspx.cs`! ✅

4. View logs in CloudWatch Console:
   - Service: CloudWatch → Log groups → `/aws/ec2/veteransprogram`
   - Search: `[level=ERROR]` to find all errors
   - Create alarm: Alert when error count > 5 per hour

**Cost:** $0.50/GB ingested + $0.03/GB stored
**Typical usage:** ~$2-5/month
**Time:** 1 hour
**Risk:** Very low

---

## Phase 2: Migrate to AWS SES (Week 3) 📧

**Why?** Better than third-party SMTP:
- ✅ No password to manage (uses IAM roles)
- ✅ $0.10 per 1,000 emails (vs likely paying more to smtp.ashersolutions.com)
- ✅ Better deliverability (99%+ inbox rate)
- ✅ Built-in bounce/complaint handling
- ✅ Integrated metrics in CloudWatch

**Action Steps:**

1. **Verify sender email:**
   - AWS Console → Amazon SES → Verified identities
   - Add email: `info@ashersolutions.com`
   - Check inbox for verification email, click link

2. **Request production access** (move out of SES sandbox):
   - SES Console → Account dashboard → Request production access
   - Fill form (usually approved within 24 hours)
   - In sandbox: Can only send to verified emails
   - In production: Can send to any email

3. **Install NuGet package:**
   ```powershell
   Install-Package AWSSDK.SimpleEmail -Version 3.7.300
   ```

4. **Attach IAM policy:**
   ```json
   {
     "Effect": "Allow",
     "Action": [
       "ses:SendEmail",
       "ses:SendRawEmail"
     ],
     "Resource": "*"
   }
   ```

5. **Update code** (choose one):

   **Option A: Update existing EmailHelper.cs** (minimal change)
   ```csharp
   // Replace SmtpClient code with SES
   using (var sesClient = new AmazonSimpleEmailServiceClient())
   {
       var request = new SendRawEmailRequest { /* ... */ };
       var response = await sesClient.SendRawEmailAsync(request);
   }
   ```

   **Option B: Use new AwsSesEmailService.cs** (already created) ✅
   ```csharp
   // In custlookuplnhp.aspx.cs SendEmail() method
   bool success = AwsSesEmailService.SendAppointmentEmailWithIcs(
       "info@ashersolutions.com",
       "Asher Solutions",
       subject,
       body,
       appointmentDate,
       location,
       toEmails,
       toNames,
       ccEmails,
       ccNames,
       out errorMessage);
   ```

**Cost:** $0.10 per 1,000 emails
**Savings:** Likely $10-50/month vs third-party SMTP
**Time:** 2-4 hours (including AWS approval wait)
**Risk:** Low (can test in sandbox first)

---

## Phase 3: High Availability (Week 4-6) 🚀

**Current:** Single EC2 instance - if it fails, site goes down

**Target Architecture:**
```
Internet → Route 53 → CloudFront → ALB → EC2 #1
                                      ├→ EC2 #2
                                      └→ EC2 #3 (Auto Scaling)
```

### 3.1 Application Load Balancer

**Benefits:**
- Health checks (auto-remove unhealthy instances)
- SSL/TLS termination (free certificate via ACM)
- Multiple availability zones
- WebSocket support (for future real-time features)

**Setup:**
1. EC2 Console → Load Balancers → Create ALB
2. Configure:
   - Name: `veteransprogram-alb`
   - Scheme: Internet-facing
   - IP type: IPv4
   - Subnets: Select 2+ availability zones
   - Security group: Allow 80, 443
3. Target group:
   - Protocol: HTTP, Port: 80
   - Health check: `/default.aspx`
4. Register current EC2 instance

**Cost:** $16/month base + $0.008/LCU-hour
**Time:** 2 hours
**Risk:** Medium (DNS cutover required)

---

### 3.2 Auto Scaling Group

**Benefits:**
- Automatic recovery if instance fails
- Scale out during high traffic
- Cost optimization (scale down at night)

**Setup:**
1. Create AMI from current EC2 instance
2. Create Launch Template:
   - AMI: From step 1
   - Instance type: Same as current
   - Security group: Allow 80 from ALB only
   - IAM role: Attach existing role with all permissions
3. Create Auto Scaling Group:
   - Min: 2, Desired: 2, Max: 5
   - Target groups: ALB from step 3.1
   - Health check: ELB
   - Scaling policy: Target CPU 70%

**Cost:** 2x EC2 cost (but you get HA)
**Savings option:** Use spot instances for 70% discount
**Time:** 4 hours
**Risk:** Medium (need to test thoroughly)

---

## Phase 4: Modern .NET Runtime (Week 7-8) ⚡

**Goal:** Run .NET 8 side-by-side with legacy Web Forms

Windows Server 2022 supports both .NET Framework 4.0 AND .NET 8 simultaneously!

### 4.1 Install .NET 8 on EC2

```powershell
# RDP to EC2 instance
# Download .NET 8 Hosting Bundle
Invoke-WebRequest -Uri "https://download.visualstudio.microsoft.com/download/pr/..." `
  -OutFile "dotnet-hosting-8.0-win.exe"

# Install
.\dotnet-hosting-8.0-win.exe /quiet /norestart

# Restart IIS
iisreset

# Verify
dotnet --version  # Should show 8.0.x
```

**Cost:** Free
**Time:** 30 minutes
**Risk:** Very low (doesn't affect existing apps)

---

### 4.2 Create New ASP.NET Core 8 API

**Use existing ETCTS project** in your solution!

```bash
# On your dev machine
cd C:\Websites\ET\ETCTS
dotnet build
dotnet run
# Test at https://localhost:5001/api/customers
```

**API Example:**
```csharp
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _repo;
    private readonly ILogger<CustomersController> _logger;

    [HttpGet]
    public async Task<ActionResult<List<CustomerDto>>> Search([FromQuery] string term)
    {
        var customers = await _repo.SearchAsync(term);
        return Ok(customers);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCustomerRequest request)
    {
        var customer = await _repo.GetByIdAsync(id);
        if (customer == null) return NotFound();

        // Map request to entity
        customer.FirstName = request.FirstName;
        // ... etc

        await _repo.UpdateAsync(customer);

        // Send email asynchronously (non-blocking)
        if (request.AppointmentDate.HasValue)
        {
            _ = Task.Run(async () => await _emailService.SendAppointmentAsync(customer));
        }

        _logger.LogInformation("Customer {Id} updated by {User}", id, User.Identity.Name);
        return NoContent();
    }
}
```

**Deploy to IIS:**
```xml
<!-- web.config in ETCTS folder -->
<configuration>
  <system.webServer>
    <handlers>
      <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" />
    </handlers>
    <aspNetCore processPath="dotnet" arguments=".\ETCTS.dll" />
  </system.webServer>
</configuration>
```

**IIS Configuration:**
- Site: `veteransprogram-api.com` (or path: `/api`)
- App Pool: No Managed Code
- Port: 5001 (or use same site with different path)

**Cost:** Free
**Time:** 1 week (if starting from scratch), but you already have ETCTS project!
**Risk:** Low (doesn't affect existing pages)

---

### 4.3 Update Web Forms to Call API

**Gradual migration** - one page at a time:

```csharp
// In custlookuplnhp.aspx.cs
protected async void btnUpdate_Click(object sender, EventArgs e)
{
    using (var client = new HttpClient())
    {
        client.BaseAddress = new Uri("https://localhost:5001");

        var request = new UpdateCustomerRequest
        {
            Id = int.Parse(lblID.Text),
            FirstName = txtfname.Text,
            // ... map all fields
        };

        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PutAsync($"/api/customers/{request.Id}", content);

        if (response.IsSuccessStatusCode)
        {
            lblresult.Text = "Updated successfully";
            GridView1.DataBind();
        }
        else
        {
            lblresult.Text = $"Error: {response.StatusCode}";
        }
    }
}
```

**Benefits:**
- API handles business logic (testable, reusable)
- Web Forms just handles UI
- Can build mobile app later using same API
- Can replace UI with Blazor/React gradually

---

## Phase 5: Background Job Processing (Week 9-10) 🔄

**Problem:** Email sending blocks UI for 1-2 seconds

**Solution:** Send emails asynchronously via SQS + Lambda

### Architecture:
```
User clicks Update
  ↓
Web Forms saves to RDS (fast - 100ms)
  ↓
Sends message to SQS queue (fast - 50ms)
  ↓
Returns success to user immediately ✅
  ↓
Lambda picks up message (within seconds)
  ↓
Lambda sends email via SES
```

### Setup:

**1. Create SQS Queue:**
```bash
aws sqs create-queue --queue-name veteransprogram-emails
```

**2. Create Lambda Function:**
```csharp
// Lambda function using .NET 8
public class EmailQueueHandler
{
    public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
    {
        foreach (var message in evnt.Records)
        {
            var emailRequest = JsonSerializer.Deserialize<EmailRequest>(message.Body);

            await _sesClient.SendEmailAsync(new SendEmailRequest
            {
                Source = emailRequest.From,
                Destination = new Destination { ToAddresses = emailRequest.To },
                Message = new Message
                {
                    Subject = new Content(emailRequest.Subject),
                    Body = new Body { Text = new Content(emailRequest.Body) }
                }
            });
        }
    }
}
```

**3. Update Web Forms:**
```csharp
// Instead of calling EmailHelper.Send() directly:
var emailRequest = new { To = recipientEmail, Subject = subject, Body = body };
await _sqsClient.SendMessageAsync(new SendMessageRequest
{
    QueueUrl = "https://sqs.us-east-1.amazonaws.com/123456/veteransprogram-emails",
    MessageBody = JsonSerializer.Serialize(emailRequest)
});
// Done! Email will be sent within seconds, but doesn't block user
```

**Cost:**
- SQS: $0.40 per million requests (essentially free)
- Lambda: $0.20 per million requests + $0.0000166667 per GB-second
- Typical: < $1/month

**Benefits:**
- ✅ Instant response to user (no waiting for email)
- ✅ Automatic retries if email fails
- ✅ Scales infinitely (Lambda handles 1 or 1000 emails)
- ✅ Dead letter queue for failed emails

---

## Cost Summary 💰

### Current Monthly Costs:
- EC2 t3.medium: ~$30/month (estimate)
- RDS: $? (you already have this)
- Third-party SMTP: $? (unknown)
- **Total: $30+ / month**

### After Phase 1-2 (Quick Wins):
- EC2: $30
- RDS: $? (unchanged)
- Secrets Manager: $0.50
- CloudWatch Logs: $3
- SES: $0.10 per 1000 emails (vs paid SMTP)
- **Total: $33.50 + savings on SMTP**

### After Phase 3 (High Availability):
- EC2 (2x instances): $60
- ALB: $16
- RDS: $? (unchanged)
- AWS services: $4
- **Total: $80 (but you get HA + auto-scaling)**

### Optimization Options:
- Use Reserved Instances: Save 40% on EC2 ($36 vs $60)
- Use Spot Instances for non-critical: Save 70%
- RDS Reserved Instance: Save 40-60%
- **Optimized HA setup: ~$60/month**

---

## Risk Assessment 🎲

| Phase | Change | Risk Level | Mitigation |
|-------|--------|------------|------------|
| 1 - Secrets | Add Secrets Manager | ⚠️ Low | Test in dev first, keep old code commented |
| 2 - SES | Replace SMTP with SES | ⚠️⚠️ Medium | Run in sandbox first, keep old code as fallback |
| 2 - CloudWatch | Add logging | ✅ Very Low | Optional, doesn't affect functionality |
| 3 - ALB | Add load balancer | ⚠️⚠️ Medium | DNS cutover required, test thoroughly |
| 3 - Auto Scaling | Multiple instances | ⚠️⚠️⚠️ High | Session state issues, need sticky sessions or Redis |
| 4 - .NET 8 API | New API alongside old | ⚠️ Low | Doesn't touch existing code |
| 5 - SQS/Lambda | Async email | ⚠️⚠️ Medium | Need monitoring to ensure delivery |

---

## Rollback Plan 🔄

For each phase, document rollback:

**Phase 1 (Secrets Manager):**
```csharp
// Keep old code commented
// var creds = AwsSecretsHelper.GetSmtpCredentials();  // NEW
var creds = new NetworkCredential("info@ashersolutions.com", "Fr3343v3r&^%");  // ROLLBACK
```

**Phase 2 (SES):**
```csharp
// Feature flag
bool useSes = ConfigurationManager.AppSettings["UseAwsSes"] == "true";
if (useSes)
    success = AwsSesEmailService.Send(...);  // NEW
else
    success = EmailHelper.Send(...);  // OLD (rollback)
```

**Phase 3 (ALB):**
- Keep old public IP in Route 53
- Can switch DNS back instantly

---

## Testing Checklist ✅

Before deploying each phase:

- [ ] Test on dev/staging EC2 instance first
- [ ] Verify IAM permissions work
- [ ] Check CloudWatch logs for errors
- [ ] Load test (if adding ALB/scaling)
- [ ] Test rollback procedure
- [ ] Document changes in wiki/runbook

---

## Next Steps - Action Plan 📋

### This Week (You):
1. Review the refactored code I created today
2. Install NuGet packages: `AWSSDK.SecretsManager`, `AWSSDK.CloudWatchLogs`
3. Create AWS Secrets Manager secret for SMTP password
4. Update IAM role attached to EC2 instance
5. Test locally on EC2, check CloudWatch logs

### Week 2:
6. Apply for SES production access
7. Verify sender email in SES
8. Test SES email sending in sandbox
9. Switch from SMTP to SES

### Week 3-4:
10. Expand ETCTS project with customer API endpoints
11. Test API locally
12. Deploy API to IIS on EC2
13. Update one Web Forms page to call API

### Month 2:
14. Create Application Load Balancer
15. Create AMI of current EC2 instance
16. Set up Auto Scaling Group
17. Cutover DNS to ALB

---

## Key Insights 💡

1. **You have modern infrastructure** - Windows Server 2022 supports everything
2. **Start small** - Phase 1 takes 1 day, huge security win
3. **Don't rewrite** - Modernize incrementally, keep Web Forms running
4. **Leverage AWS** - You're already paying for EC2, use other services
5. **Think long-term** - API backend enables future mobile/SPA apps

**My recommendation:** Do Phase 1 this week. It's low-risk, high-value, and builds momentum.
