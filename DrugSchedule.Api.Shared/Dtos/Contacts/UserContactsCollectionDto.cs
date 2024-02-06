using System.Collections.Generic;

namespace DrugSchedule.Api.Shared.Dtos;

public class UserContactsCollectionDto
{
    public List<UserContactDto> Contacts { get; set; } = new();
}