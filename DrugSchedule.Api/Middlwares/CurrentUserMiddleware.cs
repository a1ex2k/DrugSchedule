using DrugSchedule.Services;
using System.Security.Claims;
using DrugSchedule.Services.Services;
using DrugSchedule.Services.Services.Abstractions;

namespace DrugSchedule.Api.Middlwares;

public class CurrentUserMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var currentUserIdentificator = context.RequestServices.GetService<ICurrentUserIdentifier>();

        if (currentUserIdentificator?.CanBeSet != true)
        {
            await next.Invoke(context);
            return;
        }

        var claims = context.User.Claims;
        var identityGuidString = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var profileIdString = claims?.FirstOrDefault(c => c.Type == StringConstants.UserProfileIdClaimName)?.Value;
        var guidParsed = Guid.TryParse(identityGuidString, out var identityGuid);
        var longParsed = long.TryParse(profileIdString, out var profileId);

        if (guidParsed && longParsed)
        {
            currentUserIdentificator.Set(identityGuid, profileId);
        }
        else
        {
            currentUserIdentificator.SetNotAvailable();
        }

        await next.Invoke(context);
    }
}