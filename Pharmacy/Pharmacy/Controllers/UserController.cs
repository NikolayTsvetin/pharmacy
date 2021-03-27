using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Models;
using Pharmacy.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacy.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult RegisterPharmacy()
        {
            ViewBag.type = "Pharmacy";

            return View("Register");
        }

        [HttpGet]
        public IActionResult RegisterUser()
        {
            ViewBag.type = "User";

            return View("Register");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ApplicationUser() { Email = model.Email, City = model.City, UserName = model.Email };
                var result = await _userManager.CreateAsync(applicationUser, model.Password);

                if (result.Succeeded)
                {
                    // Create the respective role - User or Pharmacy
                    var roleExists = await _roleManager.RoleExistsAsync(model.Type);

                    if (!roleExists)
                    {
                        IdentityResult roleResult = await CreateRole(model.Type);

                        if (!roleResult.Succeeded)
                        {
                            foreach (var err in roleResult.Errors)
                            {
                                ModelState.AddModelError("Error from role manager:", err.Description);
                            }

                            return View(model);
                        }
                    }

                    await _userManager.AddToRoleAsync(applicationUser, model.Type);
                    await _signInManager.SignInAsync(applicationUser, false);

                    return Redirect("/Home/Index");
                }
                else
                {
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError("Error from user manager:", err.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("Error from sign in manager: ", "Invalid Login Attempt");

                    return View(model);
                }

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        private async Task<IdentityResult> CreateRole(string roleName)
        {
            IdentityRole role = new IdentityRole() { Name = roleName };
            var roleResult = await _roleManager.CreateAsync(role);

            return roleResult;
        }

        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user == null ? Json(true) : Json($"The email {email} is already taken.");
        }
    }
}
