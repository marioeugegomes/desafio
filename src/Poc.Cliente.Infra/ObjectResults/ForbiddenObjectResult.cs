using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Poc.Cliente.Infra.ObjectResults;

public class ForbiddenObjectResult : ObjectResult
{
    public ForbiddenObjectResult(object value) : base(value)
    {
       StatusCode = StatusCodes.Status403Forbidden;
    }
}
