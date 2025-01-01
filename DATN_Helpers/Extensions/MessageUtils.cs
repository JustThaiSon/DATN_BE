using DATN_Helpers.Constants;
using System.Globalization;
using System.Resources;

namespace DATN_Helpers.Extensions
{
    public static class MessageUtils
    {
        public static string GetMessage(int code, string language = "vi")
        {
            if (string.IsNullOrEmpty(language))
            {
                language = "vi"; 
            }

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);

            string codeString = code.ToString();
            string message = string.Empty;

            ResourceManager rm = new ResourceManager("DATN_Helpers.ResourceFiles.MessageResource", typeof(ResponseCodeEnum).Assembly);

            if (rm == null) return message;

            return rm.GetString(codeString) ?? string.Empty;
        }
    }
}
