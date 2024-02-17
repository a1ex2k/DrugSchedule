using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Api.Utils;
using DrugSchedule.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using DrugSchedule.Services.Services.Abstractions;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class ContactsController : ControllerBase
{
    private readonly IUserContactsService _userContactsService;

    public ContactsController(IUserContactsService userContactsService)
    {
        _userContactsService = userContactsService;
    }

    [HttpPost]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var contacts = await _userContactsService.GetContactsSimpleAsync(false, cancellationToken);
        return Ok(contacts.Adapt<UserContactsSimpleCollectionDto>());
    }
    

    [HttpPost]
    public async Task<IActionResult> GetCommon(CancellationToken cancellationToken)
    {
        var contacts = await _userContactsService.GetContactsSimpleAsync(true, cancellationToken);
        return Ok(contacts.Adapt<UserContactsSimpleCollectionDto>());
    }


    [HttpPost]
    public async Task<IActionResult> GetSingleExtended(UserIdDto dto, CancellationToken cancellationToken)
    {
        var contactsResult = await _userContactsService.GetContactAsync(dto.UserProfileId, cancellationToken);
        return contactsResult.Match<IActionResult>(
            c => Ok(c.Adapt<UserContactDto>()),
            error => BadRequest(error.ToDto()));
    }


    [HttpPost]
    public async Task<IActionResult> GetManyExtended(UserContactFilterDto dto, CancellationToken cancellationToken)
    {
        var contacts = await _userContactsService.GetContactsAsync(dto.Adapt<UserContactFilter>(), cancellationToken);
        return Ok(contacts.Adapt<UserContactsCollectionDto>());
    }


    [HttpPost]
    public async Task<IActionResult> AddOrUpdate(NewUserContactDto dto, CancellationToken cancellationToken)
    {
        var contactAddResult = await _userContactsService.AddContactAsync(dto.Adapt<NewUserContact>(), cancellationToken);
        return contactAddResult.Match<IActionResult>(
            ok => Ok("Contact edited successfully"),
            errorInput => BadRequest(errorInput.ToDto()),
            notFound => NotFound(notFound.ToDto()));
    }


    [HttpPost]
    public async Task<IActionResult> Remove(UserIdDto dto, CancellationToken cancellationToken)
    {
        var contactRemoveResult = await _userContactsService.RemoveContactAsync(dto.UserProfileId, cancellationToken);
        return contactRemoveResult.Match<IActionResult>(
            ok => Ok("Contact removed successfully"),
            notFound => NotFound(notFound.ToDto()));
    }
}