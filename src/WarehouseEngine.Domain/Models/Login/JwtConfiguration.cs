namespace WarehouseEngine.Domain.Models.Login;
public class JwtConfiguration
{
    public required string ValidAudience { get; set; }
    public required string ValidIssuer { get; set; }
    public required string Secret { get; set; }
}
