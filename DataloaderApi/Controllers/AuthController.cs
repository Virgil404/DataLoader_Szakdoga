using DataloaderApi.Dao;
using Microsoft.AspNetCore.Mvc;

namespace DataloaderApi.Controllers
{
    public class AuthController : Controller
    {
     
        
        private readonly IAuthHandling _authHandling;

        public AuthController (IAuthHandling authHandling)
        {
            _authHandling = authHandling;
        }


        [HttpPost("createUser")]
        public async Task<ActionResult<bool>> createUser(string username, string password, string role)
        {

            
            try
            {
              var result =  await _authHandling.CreateUser(username, password, role);

                if(result) return Ok();
                return BadRequest("Error while creating user");


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


    }
}
