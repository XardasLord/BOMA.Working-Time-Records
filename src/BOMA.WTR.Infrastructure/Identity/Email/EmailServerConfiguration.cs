namespace BOMA.WTR.Infrastructure.Identity.Email;

public class EmailServerConfiguration
{
    public string Server { get; set; }
    public string FromEmail { get; set; }
    public string Password { get; set; }
    public string UserName { get; set; }
    public int Port { get; set; }
    public bool UseAuthentication { get; set; }
    public bool UseTls { get; set; }
}