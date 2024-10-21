using Microsoft.EntityFrameworkCore;
using finance_control_server.Context;
using finance_control_server.Models;
using finance_control_server.DTOs;
using Microsoft.AspNetCore.Identity;
using finance_control_server.ViewModels;
using System.Text.Json;
using finance_control_server.Helpers;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;

public class AuthServices
{
    private readonly ApplicationDbContext _context;
    private readonly SignInManager<Usuario> _signInManager;
    private readonly UserManager<Usuario> _userManager;
    private readonly MailServices _mailServices;
    private readonly IConfiguration _configuration;
    private readonly TextInfo _textInfo;

    public AuthServices(SignInManager<Usuario> signInManager,
        UserManager<Usuario> userManager, MailServices mailServices,
        IConfiguration configuration, ApplicationDbContext context)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _mailServices = mailServices;
        _configuration = configuration;
        _context = context;
        _textInfo = new CultureInfo("pt-BR", false).TextInfo;
    }

    public async Task<TokenViewModel> SignIn(SignInDTO signInDTO)
    {
        try
        {
            Usuario? usuario = await _userManager.FindByEmailAsync(signInDTO.Email1);

            if (usuario != null)
            {
                var result = await _signInManager
                    .PasswordSignInAsync(usuario.UserName!.ToLower(),
                                        signInDTO.Password1,
                                        isPersistent: signInDTO.Remember1,
                                        lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    UsuarioCargo? usuarioCargo = await _context.UsuariosCargos
                        .FirstOrDefaultAsync(u => u.UserId == usuario.Id);

                    return GenerateToken(usuario, usuarioCargo);
                }
                else
                {
                    result = await _signInManager
                      .CheckPasswordSignInAsync(usuario, signInDTO.Password1, false);

                    if (result.Succeeded)
                    {
                        UsuarioCargo? usuarioCargo = await _context.UsuariosCargos
                          .FirstOrDefaultAsync(u => u.UserId == usuario.Id);

                        return GenerateToken(usuario, usuarioCargo);
                    }
                    else
                    {
                        return new TokenViewModel()
                        {
                            Authenticated = false,
                            Message = "Login ou senha incorretos!"
                        };
                    }
                }
            }
            else
            {
                return new TokenViewModel()
                {
                    Authenticated = false,
                    Message = "Login ou senha incorretos!"
                };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new TokenViewModel()
            {
                Authenticated = false,
                Message = "Ocorreu um erro inesperado!"
            };
        }
    }

    public async Task<bool> SignUp(SignUpDTO signUpDTO)
    {
        try
        {
            var usuario = new Usuario()
            {
                UserName = signUpDTO.Username1.ToLower(),
                Email = signUpDTO.Email1.ToLower(),
                EmailConfirmed = false,
                Nome = _textInfo.ToTitleCase(signUpDTO.Name1),
                Imagem = ""
            };

            var result = await _userManager.CreateAsync(usuario, signUpDTO.Password1);

            if (result.Succeeded)
            {
                result = await _userManager.AddToRoleAsync(usuario, "membro");

                if (result.Succeeded)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public async Task<bool> ConfirmEmail(SignUpDTO signUpDTO)
    {
        try
        {
            Usuario? usuario = await _userManager.FindByEmailAsync(signUpDTO.Email1);

            if (usuario != null)
            {
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(usuario);

                AuthViewModel authViewModel = new AuthViewModel(usuario.Email!, token);

                string token_confirm_email = CryptographyHelper.EncryptString(JsonSerializer.Serialize(authViewModel), "A3FD4K8A8JFGKJ34");

                MailDTO mailDTO = new MailDTO()
                {
                    Subject = "FinancClient - Confirm Email",
                    Body = string.Format("link para confirmar o email http://localhost:4200/confirm-email/{0}", token_confirm_email),
                    From = "financ@jordanc20.com.br",
                    To = new List<string>() { usuario.Email! },
                };

                _ = _mailServices.Send(mailDTO);

                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public async Task<bool> ConfirmEmail(string authViewModelJSON, string new_email)
    {
        try
        {
            string token_confirm_email = CryptographyHelper.DecryptString(authViewModelJSON, "A3FD4K8A8JFGKJ34");

            AuthViewModel? authViewModel = JsonSerializer.Deserialize<AuthViewModel>(token_confirm_email);

            if (authViewModel != null)
            {
                Usuario? usuario = await _userManager.FindByEmailAsync(authViewModel.Email1);

                if (usuario != null)
                {
                    var result = await _userManager.ChangeEmailAsync(usuario, new_email, authViewModel.Token);

                    if (result.Succeeded)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public async Task<bool> ForgotPassword(ForgotPasswordDTO forgotPasswordDTO)
    {
        try
        {
            Usuario? usuario = await _userManager.FindByEmailAsync(forgotPasswordDTO.Email1);

            if (usuario != null)
            {
                string token = await _userManager.GeneratePasswordResetTokenAsync(usuario);

                AuthViewModel authViewModel = new AuthViewModel(usuario.Email!, token);

                string token_password_reset = CryptographyHelper.EncryptString(JsonSerializer.Serialize(authViewModel), "A3FD4K8A8JFGKJ34");

                MailDTO mailDTO = new MailDTO()
                {
                    Subject = "FinancClient - Recovery Password",
                    Body = string.Format("link para redefinir a senha http://localhost:4200/forgot-password/{0}", token_password_reset),
                    From = "financ@jordanc20.com.br",
                    To = new List<string>() { usuario.Email! },
                };

                _ = _mailServices.Send(mailDTO);

                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public async Task<bool> ForgotPassword(string authViewModelJSON, string new_password)
    {
        try
        {
            string token_password_reset = CryptographyHelper.DecryptString(authViewModelJSON, "A3FD4K8A8JFGKJ34");

            AuthViewModel? authViewModel = JsonSerializer.Deserialize<AuthViewModel>(token_password_reset);

            if (authViewModel != null)
            {
                Usuario? usuario = await _userManager.FindByEmailAsync(authViewModel.Email1);

                if (usuario != null)
                {
                    var result = await _userManager.ResetPasswordAsync(usuario, authViewModel.Token, new_password);

                    if (result.Succeeded)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public async Task<bool> ValidateAsync(string token)
    {
        try
        {
            TokenValidationParameters tokenValidationParameters =
                new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidAudience = _configuration["Token:Audience"],
                    ValidIssuer = _configuration["Token:Issuer"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_configuration[Environment.GetEnvironmentVariable("JWT_KEY")!]!)
                    )
                };

            SecurityToken? validatedToken = null;

            new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out validatedToken);
            
            if (validatedToken != null)
            {
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var usuarioId = jwtSecurityToken.Claims
                        .FirstOrDefault(item => item.Type == ClaimTypes.NameIdentifier)?.Value;

                    if (usuarioId != null)
                    {
                        var usuario = await _userManager.FindByIdAsync(usuarioId);

                        if (usuario != null)
                        {
                            return true;
                        }                    
                    }
                }
            }

            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private TokenViewModel GenerateToken(Usuario usuario, UsuarioCargo? usuarioCargo = null)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id!),
            new Claim(JwtRegisteredClaimNames.UniqueName, usuario.UserName!),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email!),
            new Claim(JwtRegisteredClaimNames.Picture, usuario.Imagem!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, usuarioCargo != null ? usuarioCargo.RoleId : ""),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[Environment.GetEnvironmentVariable("JWT_KEY")!]!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow.AddHours(double.Parse(_configuration["Token:ExpireHours"]!));

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _configuration["Token:Issuer"],
            audience: _configuration["Token:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
        );

        return new TokenViewModel()
        {
            Authenticated = true,
            UserId = usuario.Id,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration,
            Message = "JWT Token OK!"
        };
    }
}
