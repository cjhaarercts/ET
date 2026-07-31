using System.Collections.Generic;

/// <summary>
/// Centralized agent email mapping service to replace hard-coded if/else chains
/// TODO: Move this data to a database table (Agents with Name, Email, GmailAlias columns)
/// </summary>
public class AgentEmailService
{
    private static readonly Dictionary<string, AgentEmailInfo> AgentEmails;

    static AgentEmailService()
    {
        AgentEmails = new Dictionary<string, AgentEmailInfo>();
        AgentEmails.Add("VPP Sharon Stangler", new AgentEmailInfo("rsstangler1@gmail.com", "rsstangler1"));
        AgentEmails.Add("VPP Richard Stangler", new AgentEmailInfo("rjsstangler@gmail.com", "rjsstangler"));
        AgentEmails.Add("Asher Sharon Stangler", new AgentEmailInfo("rsstangler1@gmail.com", "rsstangler1"));
        AgentEmails.Add("Asher Richard Stangler", new AgentEmailInfo("rjsstangler@gmail.com", "rjsstangler"));
        AgentEmails.Add("Sharon Stangler", new AgentEmailInfo("rsstangler1@gmail.com", "rsstangler1"));
        AgentEmails.Add("Richard Stangler", new AgentEmailInfo("rjsstangler@gmail.com", "rjsstangler"));
        AgentEmails.Add("Mary Jo Hudson", new AgentEmailInfo("maryjoveteransprogram@gmail.com", "maryjoveteransprogram"));
        AgentEmails.Add("GC Sharon Stangler", new AgentEmailInfo("rsstangler1@gmail.com", "rsstangler1"));
        AgentEmails.Add("GC Richard Stangler", new AgentEmailInfo("rjsstangler@gmail.com", "rjsstangler"));
    }

    private const string DefaultEmail = "cj.haarer@gmail.com";
    private const string DefaultGmailAlias = "cj.haarer";

    public static AgentEmailInfo GetAgentEmailInfo(string agentName)
    {
        if (string.IsNullOrWhiteSpace(agentName))
        {
            return new AgentEmailInfo(DefaultEmail, DefaultGmailAlias);
        }

        AgentEmailInfo info;
        if (AgentEmails.TryGetValue(agentName.Trim(), out info))
        {
            return info;
        }

        return new AgentEmailInfo(DefaultEmail, DefaultGmailAlias);
    }
}

public class AgentEmailInfo
{
    private readonly string _email;
    private readonly string _gmailAlias;

    public string Email 
    { 
        get { return _email; }
    }

    public string GmailAlias 
    { 
        get { return _gmailAlias; }
    }

    public AgentEmailInfo(string email, string gmailAlias)
    {
        _email = email;
        _gmailAlias = gmailAlias;
    }

    public string GetGmailAddress()
    {
        return string.Format("{0}@gmail.com", GmailAlias);
    }
}
