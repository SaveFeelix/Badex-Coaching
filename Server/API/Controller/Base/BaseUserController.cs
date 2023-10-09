using Microsoft.AspNetCore.Mvc;
using Server.Db;

namespace Server.API.Controller.Base;

[Route("User/[controller]/[action]")]
public class BaseUserController<TController> : BaseController<TController>
{
    public BaseUserController(DataContext database, ILogger<TController> logger) : base(database, logger)
    {
    }
}