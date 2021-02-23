using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using CroppShop.Models.AccountModel;
using CroppShop.Models.AdditionalModel;
using CroppShop.Models.EmailServices;

namespace CroppShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        public AccountController(UserManager<User> _userManager, SignInManager<User> _signInManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = registerViewModel.Email, UserName = registerViewModel.Email, Role = "user" };
                var result = await userManager.CreateAsync(user, registerViewModel.Password);
                if (result.Succeeded)
                {
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code },
                        protocol: HttpContext.Request.Scheme);
                    RegisterConfirmEmail email = new RegisterConfirmEmail();
                    await email.SendEmailAsync(registerViewModel.Email,
                        "Confirm your account", $"Confirm registration by following the <a href='{callbackUrl}'>link</a>");
                    return Content("To complete the registration, check your e-mail and follow the link provided in the letter");
                }
            }
            return View("Login", "Account");
        }
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
                return View("Error");
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await userManager.ConfirmEmailAsync(user, code);
            await userManager.AddToRoleAsync(user, "user");
            if (result.Succeeded)
                return RedirectToAction("Login", "Account");
            else
                return View("Error");
        }
        public IActionResult Login(string returnUrl = null)
        {
            LoginViewModel loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(new IdentityViewModel { LoginViewModel = loginViewModel });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, true, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(loginViewModel.ReturnUrl) && Url.IsLocalUrl(loginViewModel.ReturnUrl))
                    {
                        return Redirect(loginViewModel.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Неправильный логин или пароль");
                }
            }
            return View(loginViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
