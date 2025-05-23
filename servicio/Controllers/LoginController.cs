using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using servicio.Data;
using servicio.Models;
using servicio.Models.ModelsDTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace servicio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly MyAppContext myAppContext;
        private readonly IConfiguration _config;

        public LoginController(MyAppContext myAppContext, IConfiguration config)
        {
            this.myAppContext = myAppContext;
            _config = config;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var currentUser = GetCurrentUser();
            return Ok($"Hola { currentUser.FirtName }, tu eres un {currentUser.Role}");
        }

        [HttpPost]
        public IActionResult Login(LoginUser userLogin)
        {
            var user = Authenticate(userLogin);

            if(user != null)
            {
                //Crear el token
                var token = Generate(user);
                return Ok(token);
            }

            return NotFound("Usuario no encontrado");
        }

        private User Authenticate(LoginUser userLogin)
        {
            var currentUser = myAppContext.Users.FirstOrDefault(u => u.Username == userLogin.UserName 
                                && u.Password == userLogin.Password);

            if (currentUser != null)
            {
                return currentUser;
            }

            return null;
        }
        private string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            //Crear caims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.GivenName, user.FirtName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Role, user.Role),
            };
            
            //Crear el token
            var token = new JwtSecurityToken(
                            _config["Jwt:Issuer"],
                            _config["Jwt:Audience"],
                            claims,
                            expires: DateTime.Now.AddMinutes(60),
                            signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private User GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userclaims = identity.Claims;

                return new User
                {
                    Username = userclaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                    EmailAddress = userclaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    FirtName = userclaims.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
                    LastName = userclaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                    Role = userclaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
                };
            }
            return null;
        }
    }
}
