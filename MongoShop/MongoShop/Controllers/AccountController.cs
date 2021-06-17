using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FluentEmail.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoShop.BusinessDomain.Users;
using MongoShop.Models.Account;

namespace MongoShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IFluentEmail _emailSender;
        private readonly IUserServices _userService;


        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper,
            IFluentEmail emailSender,
            ILogger<AccountController> logger, IUserServices userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this._mapper = mapper;
            _emailSender = emailSender;
            _logger = logger;
            this._userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> UpdateInformation()
        {
            ApplicationUser user = await _userManager.FindByIdAsync(GetCurrentUserId());

            UpdateInformationViewModel profileViewModel = _mapper.Map<UpdateInformationViewModel>(user);
            return View(profileViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInformation(UpdateInformationViewModel updateInformationViewModel)
        {
            if (ModelState.IsValid)
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var user = await _userManager.FindByEmailAsync(email);

                _mapper.Map(updateInformationViewModel, user);

                await _userManager.UpdateAsync(user);
            }
            var applicationUser = _mapper.Map<ApplicationUser>(updateInformationViewModel);
            await _userService.UpdateUserAsync(GetCurrentUserId(), applicationUser);
            return View(updateInformationViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var email = User.FindFirstValue(ClaimTypes.Email);

                var loggedInUser = await _userManager.FindByEmailAsync(email);
               await _userManager.ChangePasswordAsync(loggedInUser, changePasswordViewModel.CurrentPassword,
                    changePasswordViewModel.NewPassword);
            }

            ModelState.AddModelError(string.Empty, "Invalid attempt to change password");

            return View("UpdateInformation");
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            returnUrl ??= Url.Action("Index", "Customer");

            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            ModelState.Remove("ExternalLogins");
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult loginResult
                    = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: true);

                if (loginResult == Microsoft.AspNetCore.Identity.SignInResult.Success)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return LocalRedirect(model.ReturnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Wrong email or password!");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt");

            return View(model);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl = "/customer/index")
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = "/customer/index")
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Status = true };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Add role.
                    await _userManager.AddToRoleAsync(user, UserRole.User);

                    // Add claim.
                    var claim = new Claim(ClaimTypes.Email, user.Email);
                    await _userManager.AddClaimAsync(user, claim);
                    return LocalRedirect(returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description.ToString());
                }
            }

            ModelState.AddModelError(string.Empty, "Failed to register");

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Customer");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            ApplicationUser user = await _userManager.FindByIdAsync(GetCurrentUserId());

            AccountProfileViewModel profileViewModel = _mapper.Map<AccountProfileViewModel>(user);
            return View(profileViewModel);
        }

        private string GetCurrentUserId()
        {
            return _userManager.GetUserId(HttpContext.User);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Profile(AccountProfileViewModel profileViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(profileViewModel.Email);

                _mapper.Map(profileViewModel, user);

                await _userManager.UpdateAsync(user);
            }

            return View(profileViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return View("ForgotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

                var body = $"Please follow this link to reset your password: {callbackUrl}";
                await _emailSender
                        .To(model.Email).Subject("Reset Password")
                        .Body(body, true)
                        .SendAsync();
                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        /// <summary>
        /// Renders after sending email for user who wants to reset password.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        /// <summary>
        /// Renders page for user to input information needed to reset password.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid attempt to reset password.");
                return View();
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            ModelState.AddModelError(string.Empty, "Invalid attempt to reset password.");
            return View();
        }


        /// <summary>
        /// Renders confirmation after reset password
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallBack", "Account", new { returnUrl = returnUrl });

            AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallBack(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Action("Index", "Customer");

            var loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                _logger.LogInformation("Error from external provider: {remoteError}", remoteError);
                ModelState.AddModelError(string.Empty, "Login failed");

                return View("Login", loginViewModel);
            }

            ExternalLoginResponse loginResponse = await GetExternalLoginResponseAsync(loginViewModel);

            if (loginResponse.ResponseStatus == ExternalLoginResponseStatus.Error)
            {
                _logger.LogInformation("Failed to use external login: {message}", loginResponse.Message);
                ModelState.AddModelError(string.Empty, "Login failed");
                return View("Login", loginViewModel);
            }

            _logger.LogInformation("External login successfully, redirect to {returnUrl}", returnUrl);

            return LocalRedirect(returnUrl);
        }

        private async Task<ExternalLoginResponse> GetExternalLoginResponseAsync(LoginViewModel loginViewModel)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info is null)
            {
                return new ExternalLoginResponse
                {
                    ResponseStatus = ExternalLoginResponseStatus.Error,
                    Message = "Error loading external login information"
                };
            }

            var signInResult =
                await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, true);
            if (signInResult.Succeeded)
            {
                return new ExternalLoginResponse
                {
                    ResponseStatus = ExternalLoginResponseStatus.Success,
                    Message = "Login successfully"
                };
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            if (email == null)
            {
                return new ExternalLoginResponse
                {
                    ResponseStatus = ExternalLoginResponseStatus.Error,
                    Message = $"Email Claim not received from {info.LoginProvider}"
                };
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                user = new ApplicationUser
                {
                    UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                };

                await _userManager.CreateAsync(user);

                var claim = new Claim(ClaimTypes.Email, email);
                await _userManager.AddClaimAsync(user, claim);
            }

            await _userManager.AddLoginAsync(user, info);
            await _signInManager.SignInAsync(user, false);

            return new ExternalLoginResponse
            {
                ResponseStatus = ExternalLoginResponseStatus.Success,
                Message = "Login successfully"
            };
        }
    }
}
