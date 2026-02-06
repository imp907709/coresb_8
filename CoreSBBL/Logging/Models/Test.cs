using CoreSBShared.Universal.Infrastructure.Interfaces;

namespace CoreSBBL.LogingTest.Models;

public class LoginTest : IEntityGuidId
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    public DateTime CreatedAd {get; set; }    
    
    public IEnumerable<LogginAccountTest> Accounts { get; set; }
}

public class LogginAccountTest : IEntityGuidId
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string LoginId { get; set; }
    public LoginTest Loggin { get; set; }
}
