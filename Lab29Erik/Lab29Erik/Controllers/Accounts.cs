using Lab29Erik.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lab29Erik.Controllers
{
    [Authorize(Policy = "Registered User")]
    public class AccountsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel rvm, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = rvm.Email, Email = rvm.Email, Birthday = rvm.Birthday };
                var result = await _userManager.CreateAsync(user, rvm.Password);

                if (result.Succeeded)
                {
                    const string issure = "www.Erik.com";
                    //claim list 
                    List<Claim> myClaims = new List<Claim>();

                    //claim users name is their email adress
                    Claim claim1 = new Claim(ClaimTypes.Name, user.Email, ClaimValueTypes.String, issure);
                    myClaims.Add(claim1);

                    //claim users role 
                    Claim claim2 = new Claim(ClaimTypes.Role, "RegisterdUser", ClaimValueTypes.String, issure);
                    myClaims.Add(claim2);

                    //claim for age 
                    Claim claim3 = new Claim(ClaimTypes.DateOfBirth, user.Birthday.Date.ToString(), ClaimValueTypes.Date, issure);
                    myClaims.Add(claim3);

                    //claim likes dogs
                    Claim claim4 = new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.String, issure);
                    myClaims.Add(claim4);

                    

                    var addClaims = await _userManager.AddClaimsAsync(user,myClaims);

                        await _signInManager.SignInAsync(user, isPersistent: false);
                    
                    return RedirectToAction("Index", "Home");
                }
                //ModelState.AddModelError("Password", result.Errors.ToList()[0]);e

            }

            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LogIn(LogInViewModel lvm)
        {
            if (ModelState.IsValid)
            {   
                //find user by their email
                var user = await _userManager.FindByEmailAsync(lvm.Email);
                var result = await _signInManager.PasswordSignInAsync(lvm.Email, lvm.Password, lvm.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    const string issure = "www.Erik.com";
                    //claim list 
                    List<Claim> myClaims = new List<Claim>();

                    //claim users name is their email adress
                    Claim claim1 = new Claim(ClaimTypes.Name, user.Email, ClaimValueTypes.String, issure);
                    myClaims.Add(claim1);

                    //claim users role 
                    Claim claim2 = new Claim(ClaimTypes.Role, "RegisterdUser", ClaimValueTypes.String, issure);
                    myClaims.Add(claim2);

                    //claim for age 
                    Claim claim3 = new Claim(ClaimTypes.DateOfBirth, user.Birthday.Date.ToString(), ClaimValueTypes.Date);
                    myClaims.Add(claim3);

                    //claim likes dogs
                    Claim claim4 = new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.String, issure);
                    myClaims.Add(claim4);

                    var userIdentity = new ClaimsIdentity("Registration");
                    userIdentity.AddClaims(myClaims);

                    var userPrincipal = new ClaimsPrincipal(userIdentity);

                    User.AddIdentity(userIdentity);

                    await HttpContext.SignInAsync(
                      "MyCookieLogin", userPrincipal,
                          new AuthenticationProperties
                          {
                              ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
                              IsPersistent = false,
                              AllowRefresh = false

                          });

                    return RedirectToAction("Index", "Home");

                }

            }
            string error = "you are wrong";
            ModelState.AddModelError("", error);
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AdminRegister(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AdminRegister(AdminRegisterViewModel rvm, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = rvm.Email, Email = rvm.Email };
                var result = await _userManager.CreateAsync(user, rvm.Password);

                if (result.Succeeded)
                {
                    const string issure = "www.Erik.com";
                    //claim list 
                    List<Claim> myClaims = new List<Claim>();

                    //claim users name is their email adress
                    Claim claim1 = new Claim(ClaimTypes.Name, user.Email, ClaimValueTypes.String, issure);
                    myClaims.Add(claim1);

                    //claim users role 
                    Claim claim2 = new Claim(ClaimTypes.Role, "Administrator", ClaimValueTypes.String, issure);
                    myClaims.Add(claim2);

                    //claim for age 
                    Claim claim3 = new Claim(ClaimTypes.DateOfBirth, user.Birthday.Date.ToString(), ClaimValueTypes.Date, issure);
                    myClaims.Add(claim3);

                    //claim likes dogs
                    Claim claim4 = new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.String, issure);
                    myClaims.Add(claim4);

                    await _userManager.AddClaimsAsync(user, myClaims);

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
 
                }
                //ModelState.AddModelError("Password", result.Errors.ToList()[0]);

            }

            return View();
        }
        //This loads the admin log in page.
        [AllowAnonymous]
        [HttpGet]
        public IActionResult AdminLogIn()
        {
            return View();
        }

        //This will Post the user Admin credentials.
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AdminLogIn(AdminLogInViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                //find user by their email
                var user = await _userManager.FindByEmailAsync(lvm.Email);
                var result = await _signInManager.PasswordSignInAsync(lvm.Email, lvm.Password, lvm.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    const string issure = "www.Erik.com";
                    //claim list 
                    List<Claim> myClaims = new List<Claim>();

                    //claim users name is their email adress
                    Claim claim1 = new Claim(ClaimTypes.Name, user.Email, ClaimValueTypes.String, issure);
                    myClaims.Add(claim1);

                    //claim users role 
                    Claim claim2 = new Claim(ClaimTypes.Role, "Administrator", ClaimValueTypes.String, issure);
                    myClaims.Add(claim2);

                    //claim for age 
                    Claim claim3 = new Claim(ClaimTypes.DateOfBirth, user.Birthday.Date.ToString(), ClaimValueTypes.Date);
                    myClaims.Add(claim3);

                    //claim likes dogs
                    Claim claim4 = new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.String, issure);
                    myClaims.Add(claim4);

                    var userIdentity = new ClaimsIdentity("Registration");
                    userIdentity.AddClaims(myClaims);

                    var userPrincipal = new ClaimsPrincipal(userIdentity);

                    User.AddIdentity(userIdentity);

                    await HttpContext.SignInAsync(
                       "MyCookieLogin", userPrincipal,
                           new AuthenticationProperties
                           {
                               ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
                               IsPersistent = false,
                               AllowRefresh = false

                           });


                    return RedirectToAction("Index", "Home");

                }

            }
            string error = "you are wrong";
            ModelState.AddModelError("", error);
            return View();
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
