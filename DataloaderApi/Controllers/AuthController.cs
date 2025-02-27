using DataloaderApi.Dao;
using Microsoft.AspNetCore.Mvc;
using Dataloader.Api.DTO;
using DataloaderApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
namespace DataloaderApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
     
        
        private readonly IAuthHandlingDao _authHandling;

        // private readonly TokenProvider _tokenProvider;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IdentityContext _identityContext;
        public AuthController (IAuthHandlingDao authHandling, UserManager<IdentityUser> userManager, IdentityContext identityContext)
        {
            _authHandling = authHandling;
            this.userManager = userManager;
            _identityContext = identityContext;
            // _tokenProvider = tokenProvider;
        }


        // [Authorize]
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

        [HttpGet("getuserlist")]

        public async Task<ActionResult<UserDTO>> getusers()
        {

            try
            {
                var result = await _authHandling.GetUsers();
                return Ok(result);
            }
            catch (Exception)
            {

                return BadRequest();
            }

        }

        [HttpDelete("deleteUser")]

        public async Task<IActionResult> deleteuser(String username)
        {
            // _applicationDBContext.Users
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok("User deleted");
            }
            return BadRequest("User not deleted");


        }

        [HttpPut("ChangePassword")]

      //  [Authorize]
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




    }
}
