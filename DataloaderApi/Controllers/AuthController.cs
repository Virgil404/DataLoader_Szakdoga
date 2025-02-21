using DataloaderApi.Dao;
using Microsoft.AspNetCore.Mvc;
using Dataloader.Api.DTO;
using DataloaderApi.Auth;
namespace DataloaderApi.Controllers
{
    public class AuthController : Controller
    {
     
        
        private readonly IAuthHandlingDao _authHandling;
        private readonly TokenProvider _tokenProvider;
        public AuthController (IAuthHandlingDao authHandling, TokenProvider tokenProvider)
        {
            _authHandling = authHandling;
            _tokenProvider = tokenProvider;
        }


        [HttpPost("createUser")]
        public async Task<ActionResult<bool>> createUser(string username, string password, string role)
        {

            
            try
            {
              var result =  await _authHandling.CreateUser(username, password, role);

                if(result) return Ok();
                return BadRequest(result);


            }
            catch (Exception ex) { 
            
                return BadRequest(ex.Message);
            
            }

        }
        [HttpDelete("deleteUser")]

        public async Task<ActionResult> deleteUser(string UserName)
        {

            try
            {
                var result = await _authHandling.DeleteUser(UserName);

                if (result)
                {
                    return Ok();
                }
                return BadRequest("User Cannot be deleted");



            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }


        }

        [HttpPut("ChangePassword")]

        public async Task<ActionResult<bool>> changePassword(string username, string password)
        {
            try
            {
                var result = await _authHandling.ChangePassword(username, password);

                if (result)
                {
                    Console.WriteLine("password Changed");
                    return Ok();
                }
                Console.WriteLine("Cannot Change Password");
                return BadRequest("Cannot change Password");


            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString);

                return BadRequest();


            }

        }

        [HttpPost("Login")]

        public async Task<ActionResult<AuthResponse>>Login(string username, string password)
        {

            var response = new AuthResponse();

            var user = await _authHandling.GetUserByUserName(username);

            if (user == null)
            {
                Console.WriteLine("User not Found");
                return BadRequest("user not found");

            }

            var verifypassword = BCrypt.Net.BCrypt.Verify(password, user.Password);


            if (!verifypassword)
            {
                Console.WriteLine("Password is not matching");

                return BadRequest("Wrong password");

            }

            //Create AccessToken
            var token = _tokenProvider.GenerateToken(user);
            response.AccesToken = token.AccesToken;



            return response;

        }

    }
}
