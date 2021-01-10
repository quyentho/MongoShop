using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoShop.BusinessDomain.Users;
using MongoShop.Models.Account;

namespace MongoShop.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this._mapper = mapper;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            _ = string.IsNullOrEmpty(returnUrl) ? returnUrl = "/customer/index" : returnUrl;

            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                if (result.IsLockedOut)
                {
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }


            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            _ = string.IsNullOrEmpty(returnUrl) ? returnUrl = "/customer/index" : returnUrl;

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
    }
}
