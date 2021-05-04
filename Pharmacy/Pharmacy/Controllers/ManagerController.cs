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
    public class ManagerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManagerController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult AllRoles()
        {
            var roles = _roleManager.Roles;

            return View(roles);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel role)
        {
            if (ModelState.IsValid)
            {
                IdentityRole newRole = new IdentityRole(role.RoleName);
                var result = await _roleManager.CreateAsync(newRole);

                if (result.Succeeded)
                {
                    return Redirect("/Manager/AllRoles");
                }

                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("Error from role manager", err.Description);
                }
            }

            return View(role);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id {id} cannot be found.";

                return View("NotFound");
            }

            var model = new EditRoleViewModel() { Id = id, RoleName = role.Name };

            foreach (var user in await _userManager.GetUsersInRoleAsync(role.Name))
            {
                model.Users.Add(user.UserName);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id {model.Id} cannot be found.";

                return View("NotFound");
            }

            role.Name = model.RoleName;
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("AllRoles");
            }

            foreach (var err in result.Errors)
            {
                ModelState.AddModelError("Error from role manager", err.Description);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id {id} cannot be found.";

                return View("NotFound");
            }

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return Redirect("/Manager/AllRoles");
            }

            foreach (var err in result.Errors)
            {
                ModelState.AddModelError("Error from role manager", err.Description);
            }

            return Redirect("/Manager/AllRoles");
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string id)
        {
            ViewBag.roleId = id;

            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id {id} cannot be found.";

                return View("NotFound");
            }

            var model = new List<UserRoleViewModel>();

            foreach (var user in _userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel() { UserId = user.Id, UserName = user.UserName };
                bool isUserInRole = await _userManager.IsInRoleAsync(user, role.Name);

                if (isUserInRole)
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id {id} cannot be found.";

                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                UserRoleViewModel currentUser = model[i];
                var user = await _userManager.FindByIdAsync(currentUser.UserId);
                IdentityResult result = null;
                var userIsInRole = await _userManager.IsInRoleAsync(user, role.Name);

                if (currentUser.IsSelected && !userIsInRole)
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!currentUser.IsSelected && userIsInRole)
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < model.Count - 1)
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("EditRole", new { Id = id });
                    }
                }

            }

            return RedirectToAction("EditRole", new { Id = id });
        }

        [HttpGet]
        public IActionResult AllUsers()
        {
            var users = _userManager.Users;

            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id {id} cannot be found.";

                return View("NotFound");
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return Redirect("/Manager/AllUsers");
            }

            foreach (var err in result.Errors)
            {
                ModelState.AddModelError("Error from user manager", err.Description);
            }

            return Redirect("/Manager/AllUsers");
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id {id} cannot be found.";

                return View("NotFound");
            }

            var model = new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                City = user.City,
                UserName = user.UserName,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id {model.Id} cannot be found.";
            }

            user.Email = model.Email;
            user.City = model.City;
            user.UserName = model.UserName;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Redirect("/Manager/AllUsers");
            }

            foreach (var err in result.Errors)
            {
                ModelState.AddModelError("Error in user manager", err.Description);
            }

            return View(model);
        }
    }
}
