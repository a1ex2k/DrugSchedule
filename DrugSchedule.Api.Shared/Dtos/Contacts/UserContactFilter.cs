using System.Collections.Generic;

namespace DrugSchedule.Api.Shared.Dtos;

public class UserContactFilterDto
{
    public List<long>? ContactProfileIdFilter { get; set; }

    public StringFilterDto? ContactNameFilter { get; set; }
    
    public int Skip { get; set; }

    public int Take { get; set; }
}