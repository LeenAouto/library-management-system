using Microsoft.AspNetCore.Mvc;

using Library.Entities;
using Library.DAL.Abstractions;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace MVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly IPasswordHasher _passwordHasher;

        public UsersController(IUserManager userManager, IPasswordHasher passwordHasher)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            if (user == null)
            {
                return View();
            }
            User u = _userManager.Get(user.Username);
            if (u != null)
            {
                if (_passwordHasher.Verify(u.Password, user.Password))
                {
                    var claims = new List<Claim> {
                        new Claim ("UserType", u.Type),
                        new Claim (ClaimTypes.Name, u.FirstName + " " + u.LastName)
                    };

                    var identity = new ClaimsIdentity(claims, "CookieAuth");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync("CookieAuth", claimsPrincipal);
                    return Redirect("/Home/Index");
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Loguout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return Redirect("/Home/Index");
        }

        //Add sign up method and create its page
    }
}
