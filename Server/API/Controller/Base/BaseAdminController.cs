
using Microsoft.AspNetCore.Mvc;

namespace Server.API.Controller.Base;

[Route("Admin/[controller]/[action]")]
public class BaseAdminController<TController> : BaseController<TController>
{
    public BaseAdminController(ILogger<TController> logger) : base(logger)
    {
    }
}