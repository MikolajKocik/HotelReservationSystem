using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Security.Claims;

namespace HotelReservationSystem.Web.TagHelpers.TagBased.Auth;

[HtmlTargetElement("conditional-group")]
public class ConditionalGroupTagHelper : TagHelper
{
    [HtmlAttributeName("show-for-role")]
    public string? ShowForRole { get; set; }

    [HtmlAttributeName("hide-for-role")]
    public string? HideForRole { get; set; }

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; } = default!;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        ClaimsPrincipal user = ViewContext.HttpContext.User;
        bool shouldDisplay = true;

        if (!string.IsNullOrWhiteSpace(ShowForRole))
        {
            if (!user.Identity?.IsAuthenticated ?? true)
            {
                shouldDisplay = false;
            }
            else
            {
                string[] allowedRoles = ShowForRole.Split(',', 
                    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                shouldDisplay = allowedRoles.Any(role => user.IsInRole(role));
            }
        }

        if (shouldDisplay && !string.IsNullOrWhiteSpace(HideForRole))
        {
            if (user.Identity?.IsAuthenticated ?? false)
            {
                string[] hiddenRoles = HideForRole.Split(',', 
                    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                if (hiddenRoles.Any(role => user.IsInRole(role)))
                {
                    shouldDisplay = false;
                }
            }
        }

        output.TagName = null;

        if (!shouldDisplay)
        {
            output.SuppressOutput();
        }
    }
}
