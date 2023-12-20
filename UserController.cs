using ECommerce.Models;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;



namespace ECommerce.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private static List<User> Users = new List<User>();

        /*[HttpGet]
        public List<User> Get()
        {
            return Users;
        }
        */

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginModel logindata)
        {
            Console.WriteLine(logindata.Phone,":",logindata.Password);
            if (logindata == null || string.IsNullOrEmpty(logindata.Phone) || string.IsNullOrEmpty(logindata.Password))
            {
                return BadRequest("Registered phone and password are required for login");
            }
            var user = Users.FirstOrDefault(u => u.Phone == logindata.Phone);
            if (user == null || !VerifyPassword(logindata.Password, user.Password))
            {
                return Unauthorized("Invalid credentials");
            }
            // Simulated successful login response
            return Ok("Login successful");
        }


        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            if (user == null || !ValidateUser(user))
            {
                return BadRequest("Invalid user data");
            }
            // Simulated registration process
            user.UserId = Users.Count + 1; // Assign a user ID
            user.setPassword(user.Password);
            Users.Add(user);
            // Return the registered user details
            return CreatedAtAction(nameof(Login), user);
        }

        // Custom method to validate the User object

        private bool ValidateUser(User user)
        {
            if (user == null)
            {
                return false;
            }
            return UserValidation.ValidateEmail(user.Email)
            && UserValidation.ValidateName(user.Name)
            && UserValidation.ValidatePassword(user.Password)
            && UserValidation.ValidatePhone(user.Phone);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        { 
            using (MD5 md5 = MD5.Create()) {
                byte[] inputBytes = Encoding.ASCII.GetBytes(password);
                byte[] hashedBytes = md5.ComputeHash(inputBytes);
                StringBuilder stringbuilder = new StringBuilder();

                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    stringbuilder.Append(hashedBytes[i].ToString("X2"));
                }
                string hashedInputPassword = stringbuilder.ToString();
                return hashedInputPassword==hashedPassword;
            }
        }
    }
}