using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ECommerce.Models
{
    public static class DataValidation
    {
        public static bool ValidateId(int id) { return id > 0; }
        public static bool ValidateName(string name)
        {
            // Alphanumeric (A-Z, a-z, 0-9, -, _, )
            return !string.IsNullOrEmpty(name) && Regex.IsMatch(name, @"^[a-zA-Z0-9-_ ]+$");
        }

        public static bool ValidateCategory(string category)
        {
            // A-Z, a-z
            return !string.IsNullOrEmpty(category) && Regex.IsMatch(category, @"^[a-zA-Z]+$");
        }
        public static bool ValidatePrice(decimal price){ return price > 0; }
        public static bool ValidateQuantity(int quantity){ return quantity >= 0;}
        public static bool ValidateDescription(string description){ return !string.IsNullOrEmpty(description); }
        public static bool ValidateImageURL(string imageURL){ return !string.IsNullOrEmpty(imageURL); }
    }

    public static class UserValidation
    {
        public static bool ValidateId(int id) 
        { 
            return id > 0; 
        }
        public static bool ValidateName(string name)
        {
            // Alphanumeric (A-Z, a-z, )
            return !string.IsNullOrEmpty(name) && Regex.IsMatch(name, @"^[a-zA-Z ]+$");
        }
        public static bool ValidateEmail(string Email) 
        {
            return !string.IsNullOrEmpty(Email) && new EmailAddressAttribute().IsValid(Email); 
        }

        public static bool ValidatePassword(string password) 
        {  
            return !string.IsNullOrEmpty(password) && password.Length>=8; 
        }
        
        public static bool ValidatePhone(string Phone)
        {
            return !string.IsNullOrEmpty(Phone) && Regex.IsMatch(Phone, @"^[0-9]{10}$");
        }
    }
}

