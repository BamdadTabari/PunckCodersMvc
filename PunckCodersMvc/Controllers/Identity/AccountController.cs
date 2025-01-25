using DataProvider.Assistant.Helpers;
using DataProvider.EntityFramework.Repository;
using DataProvider.Models.Command.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PunckCodersMvc.Controllers.Identity;
public class AccountController : Controller
{
    private readonly JwtTokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public AccountController(JwtTokenService tokenService, IUnitOfWork unitOfWork)
    {
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    // TODO: send email to user, Get Code from email
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromForm] PasswordResetCommand request)
    {
        var user = await _unitOfWork.UserRepo.GetUser(request.EmailOrUserName);

        if (user == null)
            return Unauthorized("Invalid credentials");


        // TODO: send email to user, Get Code from email

        user.PasswordHash = PasswordHasher.Hash(request.NewPassword);
        user.FailedLoginCount = 0;
        _unitOfWork.UserRepo.Update(user);
        await _unitOfWork.CommitAsync();

        var role = _unitOfWork.UserRoleRepo.GetUserRolesByUserId(user.Id);
        var token = _tokenService.GenerateToken(user, role.Select(x => x.Role.Title).ToList());

        return Ok(new { Token = token });
    }

    [HttpPatch("change-password")]
    public async Task<IActionResult> ChangePassword([FromForm] PasswordChangeCommand request)
    {
        var user = await _unitOfWork.UserRepo.GetUser(request.UserId);

        if (user == null)
            return Unauthorized("Invalid credentials");

        if (!PasswordHasher.Check(user.PasswordHash, request.OldPassword))
        {
            return Unauthorized("Invalid credentials");
        }
        user.PasswordHash = PasswordHasher.Hash(request.NewPassword);
        _unitOfWork.UserRepo.Update(user);
        await _unitOfWork.CommitAsync();

        var role = _unitOfWork.UserRoleRepo.GetUserRolesByUserId(user.Id);
        var token = _tokenService.GenerateToken(user, role.Select(x => x.Role.Title).ToList());

        return Ok(new { Token = token });
    }
}
