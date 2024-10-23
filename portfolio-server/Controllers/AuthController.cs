using System.Net;
using portfolio_server.DTOs;
using portfolio_server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace portfolio_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly AuthServices _authServices;
        public AuthController(AuthServices authServices)
        {
            _authServices = authServices;
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] SignInDTO signInDTO)
        {
            TokenViewModel tokenViewModel = await _authServices.SignIn(signInDTO);
            if (tokenViewModel.Authenticated)
                return Ok(tokenViewModel);
            else
                return Unauthorized(tokenViewModel.Message);
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDTO signUpDTO)
        {
            bool register = await _authServices.SignUp(signUpDTO);
            if (register)
                return Ok(register);
            else
                return NotFound();
        }

        [HttpPost("Forgot-Password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPasswordDTO)
        {
            return Ok(await _authServices.ForgotPassword(forgotPasswordDTO));
        }

        [HttpPost("Forgot-Password-Confirm")]
        public async Task<IActionResult> ForgotPasswordConfirm([FromBody] ForgotPasswordConfirmDTO forgotPasswordConfirmDTO)
        {
            return Ok(await _authServices.ForgotPassword(forgotPasswordConfirmDTO.Token1, forgotPasswordConfirmDTO.Password1));
        }

        [HttpGet("Validate")]
        public async Task<IActionResult> Validate([FromQuery] string token)
        {
            return Ok(await _authServices.ValidateAsync(token));
        }
    }
}