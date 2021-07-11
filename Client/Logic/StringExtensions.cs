using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace microcritic.Client.Logic
{
    public static class StringExtensions
    {
        public static string UnEmail(this string email)
        {
            var match = Regex.Match(email, @"^(.+?)@.+$");

            if (match.Success) return match.Groups[1].Value;
            else return email;
        }
    }
}
