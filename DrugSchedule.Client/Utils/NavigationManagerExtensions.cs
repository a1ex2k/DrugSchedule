using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System.Web;

namespace DrugSchedule.Client.Utils;

public static class NavigationManagerExtensions
{
    public static void NavigateWithParameter(this NavigationManager navManager, string page, string parameterName, string parameterValue)
    {
        var url = $"{page}?{parameterName}={HttpUtility.UrlEncode(parameterValue)}";
        navManager.NavigateTo(url, false, false);
    }

    public static void NavigateWithParameter(this NavigationManager navManager, string page, params (string Name, string Value)[] parameters)
    {
        var sb = new StringBuilder(page);
        sb.Append('?');
        foreach (var parameter in parameters)
        {
            sb.Append($"{parameter.Name}={parameter.Value}&");
        }

        sb.Remove(sb.Length - 1, 1);
        navManager.NavigateTo(sb.ToString(), false, false);
    }
}