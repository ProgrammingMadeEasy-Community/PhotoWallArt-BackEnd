using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{
    public static class StringExtension
    {
        public static string Encode(this string str, int? strLenght = null)
        {
            var encode = Convert.ToBase64String(str.GetBytes());
            if (strLenght.HasValue)
            {
               encode = encode.Take(strLenght.Value);
            }
            return encode;
        }

        public static string Take(this string str, int strLenght)
        {
            if (strLenght > str.Length)
            {
                strLenght = str.Length;
            }
            return str.Substring(0, strLenght);
        }

        public static byte[] GetBytes(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static string ToBase64String(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public static string ToJsonString(this object jsonObj)
        {
            return JsonSerializer.Serialize(jsonObj);
        }

        public static string GenerateRandomString(int maxLenght, int byteLength = 8)
        {
            var randomNumber = new byte[byteLength];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
           return randomNumber.ToBase64String()
                            .Take(maxLenght);
        }

        public static string GenerateAlhaNumericString(int charLenght)
        {
            var symbol = "@#$&_";
            var alphaNumeric = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string allowedChars = alphaNumeric + symbol;
            Random random = new();
            string str = new(Enumerable.Repeat(allowedChars, charLenght)
                                                      .Select(s => s[random.Next(s.Length)]).ToArray());

            bool isAlphanumeric = str.Any(char.IsUpper) && str.Any(char.IsLower)
                           && str.Any(char.IsDigit);
            bool containsNonAlphanumeric = symbol.Any(str.Contains);

            if (!containsNonAlphanumeric || !isAlphanumeric)
            {
                return GenerateAlhaNumericString(charLenght);
            }
            return str;
        }

        public static string RemoveNonAlphaNumeric(this string str, int? strLenght = null)
        {
            str = Regex.Replace(str, "[^a-zA-Z0-9]", "");
            if(strLenght is not null)
            {
                str = str.Take(strLenght.Value);
            }
            return str;
        }

        public static string AppendNonAlphaNumeric(this string str, int num)
        {
            num = num > 6 ? 6 : num;
            var allowedChars = "@#$%&_";
            Random random = new();
            string randomChars = new(Enumerable.Repeat(allowedChars, num)
                                                .Select(s => s[random.Next(s.Length)]).ToArray());
            return str + randomChars;
        }
    }
}
