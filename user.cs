using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;



namespace ECommerce.Models
{
    public class User
    {

        public int UserId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid phone number")]
        public required string Phone { get; set; }
        [Required(ErrorMessage = "Account type is required")]
        public string AccountType { get; set; } = "customer";


        public void setPassword(string password)
        {
            using (MD5 mD = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(password);
                byte[] hashedBytes = mD.ComputeHash(inputBytes);

                StringBuilder stringbuilder = new StringBuilder();
                for(int i=0; i<hashedBytes.Length; i++)
                {
                    stringbuilder.Append(hashedBytes[i].ToString("X2"));
                }
                Password = stringbuilder.ToString();
            }
        }

        public User(int userId, string name, string email, string password, string phone, string accountType)
        {
            UserId = userId;
            Name = name;
            Email = email;
            Password = password;
            Phone = phone;
            AccountType = accountType;
        }
    }

    public class UserLoginModel
    {
        public required string Phone { set; get; }
        public required string Password { set; get; }

        public  UserLoginModel(string phone, string password)
        {
            Phone = phone;
            Password = password;
        }
    }
}