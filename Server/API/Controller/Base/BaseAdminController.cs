using Microsoft.AspNetCore.Mvc;
using Server.Db;

namespace Server.API.Controller.Base;

[Route("Admin/[controller]/[action]")]
public class BaseAdminController<TController> : BaseController<TController>
{
    public BaseAdminController(DataContext database, ILogger<TController> logger) : base(database, logger)
    {
    }
}