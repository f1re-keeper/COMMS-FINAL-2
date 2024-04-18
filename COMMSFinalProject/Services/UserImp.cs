using COMMSFinalProject.Interfaces;
using COMMSFinalProject.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace COMMSFinalProject.Services 
{
    public class UserImp : IUser
    {
        private UserContext _context;
        private readonly AppSettings _appSettings;
        private readonly ITokenMaker _tokenParse;
        public UserImp(UserContext context, IOptions<AppSettings> appSettings, ITokenMaker tokenParse)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _tokenParse = tokenParse;
        }
        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
        
        public async Task<User> Register(NewUser regData)
        {
            var user = new User();
            user.Role = Role.User;
            user.FirstName = regData.FirstName;
            user.LastName = regData.LastName;
            user.UserName = regData.UserName;
            user.Age = regData.Age;
            user.Email = regData.Email;
            user.Salary = regData.Salary;
            user.Password = HashHelper.HashPassword(regData.Password);
            user.Token = "";
            await _context.Users.AddAsync(user);
            return user;
        }
        public async Task<User> Login(User user)
        {

            if (user == null) return null;
            string tokenString = GenerateToken(user);
            var userId = _context.Users
                        .Where(x => x.UserName == user.UserName)
                        .Select(x => x.Id)
                        .SingleOrDefault();
            var currentrecord = _context.Users.Find(userId);
            user.Token = tokenString;
            currentrecord.Token = tokenString;
            _context.Users.Update(currentrecord);
            await _context.SaveChangesAsync();
            return user;
        }

        public User Authenticate(string userName, string password)
        {

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.Users.SingleOrDefault(x => x.UserName == userName);

            if (user == null)
                return null;

            if (HashHelper.HashPassword(password) != user.Password)
                return null;

            return user;
        }
        public User GetOwnData()
        {

            var id = _tokenParse.GetUserId();
            var curr = _context.Users.Find(id);
            return curr;
        }
    }
}
