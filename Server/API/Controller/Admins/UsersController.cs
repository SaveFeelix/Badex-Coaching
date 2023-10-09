using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Dto.Users;
using Models.Result;
using Server.API.Controller.Base;
using Server.API.Controller.Interfaces;
using Server.Db;
using Server.Db.Models;
using Server.Db.Models.Types;

namespace Server.API.Controller.Admins;

public class UsersController : BaseAdminController<UsersController>, ICrudController<UserGetDto, UserCreateDto, UserUpdateDto>
{
    public UsersController(DataContext database, ILogger<UsersController> logger) : base(database, logger)
    {
    }

    [HttpGet]
    public async Task<ActionResult<BaseResult<List<UserGetDto>>>> All()
    {
        try
        {
            var users = await Database.User.Where(it => it.State != ItemState.Deactivated).ToListAsync();
            var dtoList = users.Select(it => it.ToDto()).ToList();
            return Ok(new SuccessResult<List<UserGetDto>>("Alle Benutzer wurden erfolgreich ausgelesen!", dtoList));
        }
        catch (Exception e)
        {
            return Ok(new ErrorResult(e.Message));
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BaseResult<UserGetDto>>> ById(int id)
    {
        try
        {
            var user = await Database.User.FindAsync(id);
            return user?.State is null or ItemState.Deactivated 
                ? Ok(new ErrorResult("Dieser Benutzer existiert nicht!")) 
                : Ok(new SuccessResult<UserGetDto>("Der Benutzer wurde erfolgreich ausgelesen!", user.ToDto()));
        }
        catch (Exception e)
        {
            return Ok(new ErrorResult(e.Message));
        }
    }

    [HttpPost]
    public async Task<ActionResult<BaseResult<UserGetDto>>> Create(UserCreateDto dto)
    {
        try
        {
            var existingUser = await Database.User.Where(it => it.State != ItemState.Deactivated)
                .FirstOrDefaultAsync(it => it.UserName.ToLower() == dto.UserName.ToLower());
            if (existingUser is not null)
                return Ok(new ErrorResult("Ein Benutzer mit diesem Benutzernamen existiert bereits!"));
            var user = new UserModel(dto.FirstName, dto.LastName, dto.Email, dto.Phone, dto.UserName, dto.IsAdmin,
                false);
            await Database.User.AddAsync(user);
            await Database.SaveChangesAsync();
            return Ok(new SuccessResult<UserGetDto>("Der Benutzer wurde erfolgreich erstellt!", user.ToDto()));
        }
        catch (Exception e)
        {
            return Ok(new ErrorResult(e.Message));
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<BaseResult<UserGetDto>>> Update(int id, UserUpdateDto dto)
    {
        try
        {
            var existingUser = await Database.User.FindAsync(id);
            if (existingUser?.State is null or ItemState.Deactivated)
                return Ok(new ErrorResult("Dieser Benutzer existiert nicht!"));
            existingUser.FirstName = dto.FirstName;
            existingUser.LastName = dto.LastName;
            existingUser.Email = dto.Email;
            existingUser.Phone = dto.Phone;
            existingUser.UserName = dto.UserName;
            existingUser.IsAdmin = dto.IsAdmin;
            await Database.SaveChangesAsync();
            return Ok(new SuccessResult<UserGetDto>("Der Benutzer wurde erfolgreich aktualisiert!", existingUser.ToDto()));
        }
        catch (Exception e)
        {
            return Ok(new ErrorResult(e.Message));
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<BaseResult>> Delete(int id)
    {
        try
        {
            var existingUser = await Database.User.FindAsync(id);
            if (existingUser?.State is null or ItemState.Deactivated)
                return Ok(new ErrorResult("Dieser Benutzer existiert nicht!"));
            if (existingUser.StateInsteadOfRemove())
            {
                existingUser.State = ItemState.Deactivated;
                existingUser.Changed = DateTime.UtcNow;
                existingUser.Version++;
            }
            else
                Database.User.Remove(existingUser);

            await Database.SaveChangesAsync();
            return Ok(new SuccessResult("Der Benutzer wurde erfolgreich deaktiviert!"));
        }
        catch (Exception e)
        {
            return Ok(new ErrorResult(e.Message));
        }
    }
}