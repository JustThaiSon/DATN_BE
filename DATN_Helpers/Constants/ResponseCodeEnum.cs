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
        ERR_EXISTS_PHONENUMBER = -105,
        ERR_EMAIL_EXIST = -104,
        ERR_SYSTEM = -500,
        ERR_USER_NOT_FOUND = -105,
        OTP_SENT = 106,
        ERR_INVALID_OTP = 107,
        ERR_TOKEN_INVALID = -405,
        ERR_PASSWORD = -406,
        // Code Validator (-600< and >-700)
        VLD_REQUIRED = -601,
        VLD_MAX_LENGTH = -602,
        VLD_MIN_LENGTH = -603,
        VLD_RANGE_LENGTH = -604,
        VLD_NOT_NEGATIVE = -605,
        VLD_GREATER_THAN_ZERO = -606,
        VLD_MUST_BE_INTEGER = -607
    }
}
