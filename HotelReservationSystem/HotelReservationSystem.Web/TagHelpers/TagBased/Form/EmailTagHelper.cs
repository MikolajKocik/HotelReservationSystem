using HotelReservationSystem.Core.Domain.Entities;
using Microsoft.AspNetCore.Razor.TagHelpers;
namespace HotelReservationSystem.Web.TagHelpers.TagBased.Form;

[HtmlTargetElement("email", Attributes = "guest")]
public sealed class EmailTagHelper : TagHelper
{
    public Guest Guest { get; set; } = default!;
    public string? MailTo { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "a";
        output.TagMode = TagMode.StartTagAndEndTag;

        string email = Guest?.Email ?? string.Empty;
        string domain = string.Empty;

        if (!string.IsNullOrWhiteSpace(email) && email.Contains("@"))
        {
            domain = email.Split('@').Last();
        }

        if (string.IsNullOrEmpty(domain))
        {
            output.SuppressOutput();
            return;
        }

        string address = string.IsNullOrWhiteSpace(MailTo)
            ? domain
            : $"{MailTo}@{domain}";

        output.Attributes.SetAttribute("href", $"mailto:{address}");
        output.Content.SetContent(address);
    }
}
