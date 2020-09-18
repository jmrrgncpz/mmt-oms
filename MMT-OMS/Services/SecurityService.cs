using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MMT_OMS.Services
{
    public class SecurityService
    {
        public static bool ConfirmPassword(string password, Guid salt, byte[] storedPasswordHash)
        {
            byte[] passwordSalt = Encoding.UTF8.GetBytes(salt.ToString());
            byte[] passwordHash = Hash(password, passwordSalt);

            return storedPasswordHash.SequenceEqual(passwordHash);
        }

        public static byte[] Hash(string value, byte[] salt)
        {
            var result = Hash(Encoding.UTF8.GetBytes(value), salt);
            return result;
        }

        public static byte[] Hash(byte[] value, byte[] salt)
        {
            byte[] saltedValue = value.Concat(salt).ToArray();
            var result = new SHA512Managed().ComputeHash(saltedValue);
            return result;
        }
    }
}