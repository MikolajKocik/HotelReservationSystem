using HotelReservationSystem.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Security.Claims;
using HotelReservationSystem.Application.UseCases.Auth;

namespace HotelReservationSystem.Web.TagHelpers.AttributeBased.Auth;

[HtmlTargetElement(Attributes = "asp-authorize-policy")]
public sealed class AuthorizePolicyTagHelper : TagHelper
{
    private readonly IAuthorizationService authorizationService;
    private readonly IHttpContextAccessor httpContextAccessor;

    public AuthorizePolicyTagHelper(
        IAuthorizationService authorizationService, 
        IHttpContextAccessor httpContextAccessor
        )
    {
        this.authorizationService = authorizationService;
        this.httpContextAccessor = httpContextAccessor;
    }

    [HtmlAttributeName("asp-authorize-policy")]
    public string Policy { get; set; } = string.Empty;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.RemoveAll("asp-authorize-policy");

        HttpContext? httpContext = this.httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            output.SuppressOutput();
            return;
        }

        ClaimsPrincipal? user = httpContext.User;
        AuthorizationResult? authorizationResult = await this.authorizationService
            .AuthorizeAsync(user, httpContext, Policy);

        if (!authorizationResult.Succeeded)
        {
            output.SuppressOutput();
        }
    }
}
