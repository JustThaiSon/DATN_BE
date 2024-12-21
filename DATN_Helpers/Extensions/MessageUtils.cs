using DATN_Helpers.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Helpers.Extensions
{
    public static class MessageUtils
    {
        public static string GetMessage(int code, string language = "vi")
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);

            string codeString = code.ToString();
            string message = string.Empty;

            ResourceManager rm = new ResourceManager("DATN_Helpers.ResourceFiles.MessageResource", typeof(ResponseCodeEnum).Assembly);

            if (rm == null) return message;

            return rm.GetString(codeString) ?? "";
        }
    }
}
