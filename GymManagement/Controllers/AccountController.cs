using GymManagement.BLL.ViewModels.Account;
using GymManagement.Controllers;
using GymManagement.DAL.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger logger;

        public AccountController(UserManager<ApplicationUser> userManager
            ,SignInManager<ApplicationUser> signInManager
            , ILogger<AccountController> logger)
        {
            _userManager = userManager;
           _signInManager = signInManager;
            this.logger = logger;
        }


        [HttpGet]
        public IActionResult Login() =>  View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model , CancellationToken ct )
        {
            if (!ModelState.IsValid) return View(model);

          var user= await _userManager.FindByEmailAsync(model.Email);
            if(user == null)
            {

                ModelState.AddModelError("InvalidLogin", "Invild Email or Password");
                return View(model);
            }
           var result= await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                logger.LogInformation($"User {user.UserName} Is Signed In ");
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else if (result.IsLockedOut)
            {

                ModelState.AddModelError("InvalidLogin", "The Account Is Locked , Try Again Later");
                logger.LogWarning($"User {user.UserName} Is Locked Out");
                return View(model);
            }
            else
            {


                ModelState.AddModelError("InvalidLogin", "Invild Email or Password");
                return View(model);
            }


        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
           await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
         
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        
        
        }

    }
}
