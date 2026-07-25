using API.DTOs.Auth;
using API.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AccountController(IAccountService accountService)
    : BaseApiController
{
    private readonly IAccountService _accountService = accountService;

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginDto dto)
    {
        return Ok(await _accountService.LoginAsync(dto));
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<CurrentUserDto>> Me()
    {
        return Ok(await _accountService.GetCurrentUserAsync());
    }
}