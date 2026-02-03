using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using HotelReservationSystem.Web.ViewModels;
using Microsoft.AspNetCore.RateLimiting;

public sealed class AccountController : Controller
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            return PartialView("~/Views/Shared/_RegisterForm.cshtml", model);
        }

        var user = new IdentityUser();
        user.UserName = model.Email;
        user.Email = model.Email;

        IdentityResult? result = await userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Guest");
            await signInManager.SignInAsync(user, isPersistent: false);
            return Json(new { redirectUrl = Url.Action("Index", "Home") ?? "/" });
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        Response.StatusCode = StatusCodes.Status400BadRequest;
        return PartialView("~/Views/Shared/_RegisterForm.cshtml", model);
    }

    [HttpPost]
    [EnableRateLimiting("LoginPolicy")]
    public async Task<IActionResult> Login([FromForm] LoginViewModel model, [FromForm] string? returnUrl = null)
    {
        ViewData["IsModalLogin"] = true;

        if (!ModelState.IsValid)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            return PartialView("~/Views/Shared/_LoginForm.cshtml", model);
        }

        Microsoft.AspNetCore.Identity.SignInResult? result = await signInManager.PasswordSignInAsync(
            model.Email,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: false
        );

        if (result.Succeeded)
        {
            string redirectUrl;
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                redirectUrl = returnUrl;
            }
            else
            {
                redirectUrl = Url.Action("Index", "Home") ?? "/";
            }

            return Json(new { redirectUrl });
        }

        ModelState.AddModelError(string.Empty, "Nieprawidłowy email lub hasło.");
        Response.StatusCode = StatusCodes.Status400BadRequest;
        return PartialView("~/Views/Shared/_LoginForm.cshtml", model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult AccessDenied(string returnUrl)
    {
        return View();
    }
}
