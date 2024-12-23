using System.Text.RegularExpressions;

namespace DATN_Helpers.Extensions
{
    public static class StringExtension
    {
        public static bool IsValidEmail(this string email)
        {
            Regex rx = new Regex(
            @"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$");
            return rx.IsMatch(email);
        }
        public static bool IsValidPhoneNumber(this string phoneNumber)
        {
            Regex rx = new Regex(@"^(0[1-9]{1}[0-9]{8})$");

            return rx.IsMatch(phoneNumber);
        }
    }
}
