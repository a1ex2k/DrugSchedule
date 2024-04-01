using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Networking;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Components;

public partial class UsersList
{
    [Inject] public IApiClient ApiClient { get; set; } = default!;

    [Parameter] public EventCallback<PublicUserDto> OnSelect { get; set; }
    [Parameter] public bool Selectable { get; set; }
    [Parameter] public string SelectButtonText { get; set; } = "Select";

    private List<PublicUserDto> Users { get; set; } = new();
    private string? SearchValue { get; set; }


    private async Task LoadUsersAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchValue))
        {
            Users = null;
            return;
        }

        var result = await ApiClient.SearchUserAsync(new UserSearchDto { Take = 5, UsernameSubstring = SearchValue });
        Users = result.ResponseDto.Users;
    }

    private async Task SearchValueChanged(string value)
    {
        SearchValue = value;
        if (SearchValue.Length < 3) return;
        await LoadUsersAsync();
    }
}