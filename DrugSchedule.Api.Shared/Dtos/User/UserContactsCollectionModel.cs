using System.Collections.Generic;

namespace DrugSchedule.Api.Shared.Dtos;

public class UserContactsCollectionDto
{
    public required long UserId { get; set; }

    public List<UserContactDto> Contacts { get; set; } = new();
}