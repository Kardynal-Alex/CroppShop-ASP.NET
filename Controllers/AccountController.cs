using Microsoft.AspNetCore.Mvc;
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
        private readonly RoleManager<IdentityRole> roleManager;
        public AccountController(UserManager<User> _userManager, SignInManager<User> _signInManager, RoleManager<IdentityRole> _roleManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
        }
        public IActionResult Error() => View();
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            User user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                await userManager.DeleteAsync(user);
            }
            return RedirectToAction("AdminList", "BossAdmin");
        }
        public IActionResult ConfirmEmailContentView()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterAdmin(AdminRegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid) 
            {
                User user = new User { Email = registerViewModel.Email, UserName = registerViewModel.Email, Surname = registerViewModel.Surname, Name = registerViewModel.Name, Role = "admin" };
                var result = await userManager.CreateAsync(user, registerViewModel.Password);
                if (result.Succeeded) 
                {
                    await userManager.AddToRoleAsync(user, "admin");
                    return RedirectToAction("AdminList", "BossAdmin");
                }
            }
            return RedirectToAction("AdminList", "BossAdmin");
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = registerViewModel.Email, UserName = registerViewModel.Email, Surname = registerViewModel.Surname, Name = registerViewModel.Name, Role = "user" };
                var result = await userManager.CreateAsync(user, registerViewModel.Password);
                if (result.Succeeded)
                {
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code },
                        protocol: HttpContext.Request.Scheme);
                    RegisterConfirmEmail email = new RegisterConfirmEmail();
                    await email.SendEmailAsync(registerViewModel.Email,
                        "Confirm your account", $"Confirm registration by following the <a href='{callbackUrl}'>link</a>");
                    return RedirectToAction("ConfirmEmailContentView", "Account");
                }
            }
            return View("Login", "Account");
        }
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
                return View("Error", "Account");
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error", "Account");
            }
            var result = await userManager.ConfirmEmailAsync(user, code);
            await userManager.AddToRoleAsync(user, "user");
            if (result.Succeeded)
                return RedirectToAction("Login", "Account");
            else
                return View("Login", "Account");
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
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null && !(await userManager.IsEmailConfirmedAsync(user)))
                {
                    return View("ForgotPasswordConfirmation", "Account");
                }
                var code = await userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code, email = model.Email },
                    protocol: HttpContext.Request.Scheme);
                RegisterConfirmEmail emailService = new RegisterConfirmEmail();
                await emailService.SendEmailAsync(model.Email,
                    "Reset password", $"To reset your password, follow the <a href='{callbackUrl}'>link</a>");
                return RedirectToAction("ConfirmEmailContentView", "Account");
            }
            return View(model);
        }
        [AllowAnonymous]
        public IActionResult ResetPassword(string email, string code = null)
        {
            ViewBag.Email = email;
            return code == null ? View("Error", "Account") : View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return View("Login", "Account");
                }
                var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            return View();
        }
    }
}
