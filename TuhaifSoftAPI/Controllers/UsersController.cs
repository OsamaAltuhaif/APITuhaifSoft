using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TuhaifSoftAPI.Data;
using TuhaifSoftAPI.EmilSender;
using TuhaifSoftAPI.Models;
using TuhaifSoftAPI.Models.DTO;


namespace TuhaifSoftAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {








        private readonly UserManager<Users> _usermanager;
        private readonly TuhaifSoftAPIDBContext _db;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UsersController> logger;
        private readonly IEmailSender _emailSender;

        public UsersController(ILogger<UsersController> loger, IConfiguration _configuration, TuhaifSoftAPIDBContext _db, UserManager<Users> _userManager, IEmailSender _emailSender)
        {
            this._configuration = _configuration;
            this._usermanager = _userManager;
            logger = loger;
            this._db = _db;
            this._emailSender = _emailSender;
        }


          [HttpPost("Register")]
          public async Task<IActionResult> Register([FromBody] UsersDTO model)
          {
              var user = new Users {Email = model.Email };
              var result = await _usermanager.CreateAsync(user, model.Password);

              if (!result.Succeeded)
              {
                  return BadRequest(result.Errors);
              }

              var verfication = await _usermanager.GenerateEmailConfirmationTokenAsync(user);
              //var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { token, email = user.Email }, Request.Scheme);

              // إرسال البريد الإلكتروني (استخدم خدمة بريد إلكتروني مثل SendGrid)
              await _emailSender.SendEmailAsync(user.Email, verfication);

              return Ok("Registration successful! Please confirm your email.");
          }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string email, string code)
          {
              var user = await _usermanager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("Invalid email.");
            }
              var result = await _usermanager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                return BadRequest("Email confirmation failed.");
            }
              return Ok("Email confirmed successfully!");
          }








        [HttpPost("Registernew")]
        public IActionResult SignUp([FromBody] UsersDTO usersDTO)
        {
            if (_db.Users.FirstOrDefault(us => us.Email == usersDTO.Email) != null)
            {
                ModelState.AddModelError("EmilError", "This Email is Ready");
                return BadRequest(ModelState);
            }
            if (usersDTO.Email == null || usersDTO.Password == null)
            {
                ModelState.AddModelError("EmilError", "Add Correct Data");
                return BadRequest(ModelState);
            }
            else
            {
                Users user = new Users()
                {
                    Id=_db.Users.OrderByDescending(uss => uss.Id).FirstOrDefault().Id+1,
                    Email = usersDTO.Email,
                    UserName=usersDTO.Email.Split('@')[0].ToString(),
                    Password = usersDTO.Password,
                };
                _db.Users.Add(user);
                _db.SaveChanges();
                return Ok();
            }
        }


        private string GeneratorRfreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber);

        }



        [HttpPost("SignIn")]
        public IActionResult logIn(UsersDTO usersDTO)
        {

            if (_usermanager.Users.FirstOrDefault(us => us.Email == usersDTO.Email && us.Password == usersDTO.Password) != null)
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, usersDTO.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),


                };

                var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
                var creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
                var expiresIn = 1;


                var Token = new JwtSecurityToken(
                    issuer: _configuration["JwtSettings:Issuer"],
                    audience: _configuration["JwtSettings:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);
                var refreshToken = GeneratorRfreshToken();

                if (_db.RefreshTokens.FirstOrDefault(us => us.UserId == (_usermanager.Users.FirstOrDefault(use =>use.Email == usersDTO.Email && use.Password == usersDTO.Password).Id)) == null)
                {
                    var refreshTokenrRecord = new RefreshTokens
                    {
                        UserId = _usermanager.Users.FirstOrDefault(us => us.Email == usersDTO.Email && us.Password == usersDTO.Password).Id,
                        Token = refreshToken,
                    };
                    _db.RefreshTokens.Add(refreshTokenrRecord);
                    _db.SaveChanges();
                }
                else
                {
                    var _updateRefresh=_db.RefreshTokens.FirstOrDefault(refsh=>refsh.UserId==(_usermanager.Users.FirstOrDefault(us => us.Email == usersDTO.Email && us.Password == usersDTO.Password).Id));

                    _updateRefresh.Token = refreshToken;
                    _db.RefreshTokens.Update(_updateRefresh);

                    _db.SaveChanges();
                    
                }



                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(Token),
                    tokenType = "Bearer",
                    expiresIn = expiresIn * 60,
                    refreshToken = refreshToken,
                });
            }
            logger.LogWarning($"There is tying with {usersDTO.Email}");
            return Unauthorized() ;

        }

        [HttpPost("refresh-token")]
        public IActionResult RefreshToken([FromBody] RefreshTokenReuestDTO request)
        {
            var storedToken = _db.RefreshTokens.FirstOrDefault(rftn => rftn.Token == request.Token);
            if (storedToken == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid or expired refrsh Token"
                });
            }
            else /*(DateTime.Parse(storedToken.ExpiryDate) < DateTime.Now)*/
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, storedToken.UserId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),


                };

                var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
                var creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
                var expiresIn = 1;


                var Token = new JwtSecurityToken(
                    issuer: _configuration["JwtSettings:Issuer"],
                    audience: _configuration["JwtSettings:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);
                var refreshToken = GeneratorRfreshToken();

                storedToken.Token = refreshToken;
                _db.RefreshTokens.Update(storedToken);
                _db.SaveChanges();
                

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(Token),
                    tokenType = "Bearer",
                    expiresIn = expiresIn * 60,
                    refreshToken = refreshToken,
                });

            }
            return Ok();

        }
    }
}
