using Microsoft.AspNetCore.Mvc;
using Dataloader.Api.DTO;
using DataloaderApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using DataloaderApi.Dao.Interfaces;
namespace DataloaderApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {


        private readonly IAuthHandlingDao _authHandling;

        // private readonly TokenProvider _tokenProvider;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IdentityContext _identityContext;
        public AuthController(IAuthHandlingDao authHandling, UserManager<ApplicationUser> userManager, IdentityContext identityContext)
        {
            _authHandling = authHandling;
            this.userManager = userManager;
            _identityContext = identityContext;
            // _tokenProvider = tokenProvider;
        }


        // [Authorize]
        [HttpPost("createUser")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> createUser(RegisterDTO registerDTO)
        {


            try
            {
                var result = await _authHandling.CreateUser(registerDTO);

                if (result) return Ok();
                return BadRequest(result);


            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }

        [HttpGet("userprofile")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> userProfile()
        {

            var currentuser = await userManager.GetUserAsync(User);
            if (currentuser == null)
            {
                return BadRequest("User not found");
            }
            
            var userroles = await userManager.GetRolesAsync(currentuser);

            var user = new UserDTO
            {
                username = currentuser.UserName,
                email = currentuser.Email,
                Role = string.Join(",", userroles)
            };

            return Ok(user);
        }


        [HttpGet("getuserlist")]
        [Authorize]
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
        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [HttpPut("changeRole")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> changeRole(string username, string role)
        {
            try
            {
                var result = await _authHandling.changeRole(username, role);
                if (result)
                {
                    Console.WriteLine("Role Changed");
                    return Ok();
                }
                Console.WriteLine("Cannot Change Role");
                return BadRequest("Cannot change Role");



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString);
                return BadRequest();
            }
        }
    }
}
