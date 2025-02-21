
using System.Reflection.Metadata.Ecma335;
using BCrypt.Net;
using DataloaderApi.Data;
using Microsoft.EntityFrameworkCore;

namespace DataloaderApi.Dao
{
    public class AuthHandlingDao : IAuthHandlingDao


    {

        private readonly Applicationcontext _context;

        public AuthHandlingDao(Applicationcontext context)
        {
            _context = context;
        }

        public async Task<bool> CreateUser(string username, string password , string Role)
        {

            try
            {
                var userExists = await userExitsWithUserName(username);

                if (userExists) {
                    Console.WriteLine("user Alredy exist");
                    return false;
                
                }


                var hashedpassword = BCrypt.Net.BCrypt.HashPassword(password);

                var user = new User 
                { 
                    UserID = username,
                    Password = hashedpassword,
                    Role = Role
                
                };

                await _context.AddAsync(user);
                _context.SaveChanges();
                Console.WriteLine("User Created Succesfully");
                return true;
            }
            catch (Exception ex) { 
            
                Console.WriteLine("Error While Creating User "+ex.ToString());
                return false;
            
            }
        }


        public async Task<bool> ChangePassword (string username,string Password)
        {

            var user = await GetUserByUserName(username);

            if (user == null) 
            {
                Console.WriteLine("UserNotFound");
                return false; 
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(Password);
          await  _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUser(string username)
        {
            var user = await GetUserByUserName(username);
            if (user==null)
            {
                Console.WriteLine("UserNotFound");
                return false;

            }
            _context.Remove(user);
            _context.SaveChanges();
            return true;    
        }





        public async Task<bool> userExitsWithUserName(string username)
        {
            var userexist =  from u in _context.Users where u.UserID == username select u;
            return  await userexist.AnyAsync();
        }


        public async Task<User>? GetUserByUserName(string username)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID.Equals(username));
            return user;
        }


    }
}
