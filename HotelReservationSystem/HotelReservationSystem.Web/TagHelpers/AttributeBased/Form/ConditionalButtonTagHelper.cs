using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HotelReservationSystem.Web.TagHelpers.AttributeBased.Form;

[HtmlTargetElement("button", Attributes = "auth-required")]
public class ConditionalButtonTagHelper : TagHelper
{
    [HtmlAttributeName("auth-required")]
    public bool AuthRequired { get; set; }

    [HtmlAttributeName("is-authenticated")]
    public bool IsAuthenticated { get; set; }

    [HtmlAttributeName("auth-text")]
    public string? AuthText { get; set; }

    [HtmlAttributeName("submit-text")]
    public string? SubmitText { get; set; }

    [HtmlAttributeName("modal-target")]
    public string? ModalTarget { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (!AuthRequired)
        {
            return;
        }

        if (!IsAuthenticated)
        {
            output.Attributes.SetAttribute("type", "button");
            output.Attributes.SetAttribute("data-bs-toggle", "modal");
            output.Attributes.SetAttribute("data-bs-target", this.ModalTarget ?? "#loginModal");
            output.Content.SetContent(this.AuthText ?? "Zaloguj się");
        }
        else
        {
            output.Attributes.SetAttribute("type", "submit");
            output.Content.SetContent(this.SubmitText ?? "Zatwierdź");
        }
    }
}
