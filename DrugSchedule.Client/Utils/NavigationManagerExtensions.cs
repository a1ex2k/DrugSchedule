using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System.Web;
using System.Numerics;

namespace DrugSchedule.Client.Utils;

public static class NavigationManagerExtensions
{
    public static void NavigateWithParameter<T>(this NavigationManager navManager, string page, string parameterName, T parameterValue)
        where T : notnull
    {
        var url = $"{page}?{parameterName}={parameterValue}";
        navManager.NavigateTo(url, false, false);
    }

    public static void NavigateWithBoolParameter(this NavigationManager navManager, string page, string parameterName)
    {
        var url = $"{page}?{parameterName}=true";
        navManager.NavigateTo(url, false, false);
    }
}