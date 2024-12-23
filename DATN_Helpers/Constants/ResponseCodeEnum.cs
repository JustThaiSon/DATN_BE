namespace DATN_Helpers.Constants
{
    public enum ResponseCodeEnum
    {
        SUCCESS = 200,
        ERR_WRONG_INPUT = -99,
        ERR_WRONG_USERNAME_PASS = -100,
        ERR_VALUE_NOT_EXIST = -101,
        ERR_INVALID_EMAIL = -102,
        ERR_INVALID_PHONENUMBER = -103,
        ERR_EMAIL_EXIST = -104,
        ERR_SYSTEM = -500,
        ERR_USER_NOT_FOUND = -105,
        OTP_SENT = 106,
        ERR_INVALID_OTP = 107,
    }
}
