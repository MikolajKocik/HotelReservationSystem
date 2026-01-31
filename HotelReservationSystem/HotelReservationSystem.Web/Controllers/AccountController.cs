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
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = new IdentityUser { UserName = model.Email, Email = model.Email };
        IdentityResult? result = await userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return View(model);
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    [EnableRateLimiting("LoginPolicy")]
    public async Task<IActionResult> Login([FromForm] LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        Microsoft.AspNetCore.Identity.SignInResult? result = await signInManager.PasswordSignInAsync(
            model.Email,
            model.Password,
            model.RememberMe, 
            false
        );

        if (result.Succeeded)
            return RedirectToAction("Index", "Home");

        ModelState.AddModelError("", "Invalid login attempt.");
        return View(model);
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
