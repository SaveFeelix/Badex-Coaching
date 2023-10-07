using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.API.Controller.Base;

//[Authorize]
[ApiController]
[Route("Global/[controller]/[action]")]
public class BaseController<TController> : ControllerBase
{
    public BaseController(ILogger<TController> logger)
    {
        Logger = logger;
    }

    public ILogger<TController> Logger { get; }
}