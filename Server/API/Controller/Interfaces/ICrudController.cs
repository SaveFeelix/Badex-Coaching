using Microsoft.AspNetCore.Mvc;
using Models.Result;

namespace Server.API.Controller.Interfaces;

public interface ICrudController<TGetDto, in TCreateDto, in TUpdateDto>
{
    public Task<ActionResult<BaseResult<List<TGetDto>>>> All();
    public Task<ActionResult<BaseResult<TGetDto>>> ById(int id);
    public Task<ActionResult<BaseResult<TGetDto>>> Create(TCreateDto dto);
    public Task<ActionResult<BaseResult<TGetDto>>> Update(int id, TUpdateDto dto);
    public Task<ActionResult<BaseResult<TGetDto>>> Delete(int id);
}