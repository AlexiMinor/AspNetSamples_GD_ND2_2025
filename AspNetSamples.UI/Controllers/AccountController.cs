using AspNetSamples.Mappers;
using AspNetSamples.Models;
using AspNetSamples.Services.Abstractions;
using AspNetSamples.UI.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AspNetSamples.Core.Dto;
using Hangfire;
using Microsoft.AspNetCore.Authentication;

namespace AspNetSamples.UI.Controllers;

public class AccountController : Controller
{
    private readonly IUserService _userService;
    private readonly UserMapper _userMapper;

    public AccountController(IUserService userService, 
        UserMapper userMapper)
    {
        _userService = userService;
        _userMapper = userMapper;
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> LoginProcessing(LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Login", model);
        }

        var userDto = await _userService.TryToLoginUserAsync(model.Email, model.Password);

        if (userDto == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login or password");
            return View("Login", model);
        }

        await LoginUser(userDto);

        return RedirectToAction("Index", "Home");
    }

  

    [HttpGet]
    public IActionResult Register()
    {
        //ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegisterProcessing(RegisterModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return View("Register", model);
        }

        var userDto = _userMapper.RegisterModelToUserDto(model);

        var registeredUser = await _userService.RegisterUserAsync(userDto);

        //var jobId = BackgroundJob.Enqueue(
        //    () => _emailService.SendRegistrationConfirmMail(userDto.Email)); // Fire and forget

        await LoginUser(registeredUser);

        return RedirectToAction("Index", "Home");
    }


    [HttpGet]
    public IActionResult Logout()
    {
        return View();
    }

    [HttpPost]
    public IActionResult LogoutProcessing(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
     
        return View();
    }

    [HttpGet]
    public IActionResult Manage()
    {
        return Ok();
    }

    private async Task LoginUser(UserDto userDto)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Email, userDto.Email),
            new Claim(ClaimTypes.Name, userDto.Name),
            new Claim(ClaimTypes.Role, userDto.Role.Name),
            new Claim("id", userDto.Id.ToString()),
            //new Claim("custom_claim", "custom_value")
        };

        //userDto.Roles.Select(roleDto => new Claim(ClaimTypes.Role, roleDto.Name)).ToArray();
        //claims.AddRange(userDto.Roles.Select(roleDto => new Claim(ClaimTypes.Role, roleDto.Name)).ToArray());
        var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(claimIdentity);
        await HttpContext.SignInAsync(principal);
    }
}