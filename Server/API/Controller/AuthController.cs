using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Dto.Users;
using Models.Result;
using Server.API.Controller.Base;
using Server.Db;
using Server.Db.Models.Types;
using Server.Settings;

namespace Server.API.Controller;

public class AuthController : BaseController<AuthController>
{
    private JwtSettings Settings { get; }
    public AuthController(JwtSettings settings, DataContext database, ILogger<AuthController> logger) : base(database, logger)
    {
        Settings = settings;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<BaseResult<string>>> Login(UserLoginDto dto)
    {
        var user = await Database.User.Where(it => it.State == ItemState.Activated)
            // ReSharper disable once SpecifyStringComparison
            .FirstOrDefaultAsync(it => it.UserName.ToLower() == dto.Username.ToLower());

        if (user is null)
            return Ok(new ErrorResult("Ein Benutzer mit diesem Benutzer wurde nicht gefunden!"));
        var hash = await user.GenerateHash(dto.Password, true);
        if (!hash.SequenceEqual(user.Hash))
            return Ok(new ErrorResult("Ein falsches Passwort wurde angegeben!"));
        var token = user.GenerateToken(Settings);
        return Ok(new SuccessResult<string>("Login erfolgreich!", token));
    }

    [HttpGet]
    public ActionResult<BaseResult> Check()
    {
        return Ok(new SuccessResult("Token gültig!"));
    }
}