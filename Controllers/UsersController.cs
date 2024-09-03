using ERP_MaxysHC.Maxys.Data.Repositories;
using ERP_MaxysHC.Maxys.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ERP_MaxysHC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public IConfiguration _configuration;
        public UsersController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _userRepository.GetAllUsers());
        }

        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UsersModel userModel)
        {
            if (userModel == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _userRepository.UpdateUser(userModel);
            return NoContent();
        }

        [HttpPost]
        [Route("Login")]
        public dynamic LoginAsync([FromBody] UsersLogin userModel)
        {
            var dat = JsonConvert.SerializeObject(userModel);
            var data = JsonConvert.DeserializeObject<dynamic>(dat.ToString());
            string user = data.User.ToString();
            string email = data.Email.ToString();
            string password = data.Password.ToString();
            string ePassword = UserRepository.GetSHA256(password);
            UsersModel _user = _userRepository.GetAllUsers().Result.Where(x => x.User == user && x.Password == ePassword || x.Email == email && x.Password == ePassword).FirstOrDefault();

            if (_user == null)
            {
                return new
                {
                    success = false,
                    message = "Usuario o contraseña incorrectos",
                    result = _user
                };
            }

            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
        new Claim("Id_user", _user.Id_user.ToString()),
        new Claim("Password", _user.Password.ToString()),
        new Claim("Position", _user.Position.ToString()),
    };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                        jwt.Issuer,
                        jwt.Audience,
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(60),
                        signingCredentials: singIn
                        );
            return new
            {
                success = true,
                message = "Token generado correctamente",
                result = new JwtSecurityTokenHandler().WriteToken(token),
            };
        }
    }
    
}
