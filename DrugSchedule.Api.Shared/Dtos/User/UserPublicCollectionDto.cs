using System.Collections.Generic;

namespace DrugSchedule.Api.Shared.Dtos;

public class UserPublicCollectionDto
{
    public List<PublicUserDto> Users { get; set; } = new();
}