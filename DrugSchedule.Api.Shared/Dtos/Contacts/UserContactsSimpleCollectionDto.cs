using System.Collections.Generic;

namespace DrugSchedule.Api.Shared.Dtos;

public class UserContactsSimpleCollectionDto
{
    public List<UserContactSimpleDto> Contacts { get; set; } = new();
}