using Swashbuckle.AspNetCore.Filters;
using WarehouseEngine.Domain.Models.Login;

namespace WarehouseEngine.Api.Examples;
public class LoginExample : IExamplesProvider<Login>
{
    public Login GetExamples()
    {
        return new Login { Username = "demo", Password = "P@ssword1" };
    }
}
