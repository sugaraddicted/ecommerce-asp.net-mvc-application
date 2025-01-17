﻿using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MovieTicket.Data;
using MovieTicket.Models;
using Microsoft.AspNetCore.Mvc;
using MovieTicket.Data.ViewModels;
using MovieTicket.Data.Static;




namespace MovieTicket.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager; 
            _context = context; 
        }

        public async Task<IActionResult> Users()
        {
            var user = await _context.Users.ToListAsync();
            return View(user);
        }

        public IActionResult Login() => View(new LoginVM());


        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);

            if(user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Movies");
                    }

                }
                TempData["Error"] = "Wrong credentials. Plesase, try again";
                return View(loginVM);
            }

            TempData["Error"] = "Wrong credentials. Please, try again!";
            return View(loginVM);
         }


        public IActionResult Register() => View(new RegisterVM());
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);

            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerVM);
            }

            var newUser = new ApplicationUser()
            {
                FullName = registerVM.FullName,
                Email = registerVM.EmailAddress,
                UserName = registerVM.EmailAddress
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
                return View("RegisterCompleted");
            }
            else
            {
                // Handle errors if user creation failed
                TempData["Error"] = "Failed to create user (Password requires non alphanumeric, password requires upper)";
                return View(registerVM);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Movies");
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            return View();
        }

    }
}
