using Microsoft.AspNetCore.Mvc;

namespace Server.API.Controller.Base;

[Route("User/[controller]/[action]")]
public class BaseUserController<TController> : BaseController<TController>
{
    public BaseUserController(ILogger<TController> logger) : base(logger)
    {
    }
}