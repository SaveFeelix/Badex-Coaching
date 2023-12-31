using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Db;

namespace Server.API.Controller.Base;

[Authorize]
[ApiController]
[Route("Global/[controller]/[action]")]
[Produces(MediaTypeNames.Application.Json)]
public class BaseController<TController> : ControllerBase
{
    public BaseController(DataContext database, ILogger<TController> logger)
    {
        Database = database;
        Logger = logger;
    }

    protected ILogger<TController> Logger { get; }
    protected DataContext Database { get; }
}