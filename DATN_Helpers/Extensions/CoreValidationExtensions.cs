using DATN_Helpers.Constants;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Helpers.Extensions
{
    public static class CoreValidationExtensions
    {
        private static string _langCode;

        public static void Initialize(IConfiguration configuration)
        {
            _langCode = configuration["ProjectSettings:LanguageCode"] ?? "vi";
        }

        public static IRuleBuilderOptionsConditions<T, R> NotNullOrEmpty<T, R>(this IRuleBuilder<T, R> ruleBuilder)
        {
            string msg = MessageUtils.GetMessage((int)ResponseCodeEnum.VLD_REQUIRED, _langCode);

            return ruleBuilder.Custom((x, y) =>
            {
                if (x == null || (typeof(R) == typeof(string) && string.IsNullOrEmpty(x.ToString())))
                {
                    y.AddFailure(y.DisplayName, string.Format(msg, y.DisplayName));
                }
                else if (x is Guid guid && guid == Guid.Empty)
                {
                    y.AddFailure(y.DisplayName, $"{y.DisplayName} cannot be an empty GUID.");
                }
            });
        }

        public static IRuleBuilderOptionsConditions<T, R> MaxLength<T, R>(this IRuleBuilder<T, R> ruleBuilder, int maxLength)
        {
            string msg = MessageUtils.GetMessage((int)ResponseCodeEnum.VLD_MAX_LENGTH, _langCode);

            return ruleBuilder.Custom((x, y) =>
            {
                if (x != null && !string.IsNullOrEmpty(x.ToString()) && x.ToString().Length > maxLength)
                    y.AddFailure(y.DisplayName, string.Format(msg, maxLength));
            });
        }

        public static IRuleBuilderOptionsConditions<T, R> MinLength<T, R>(this IRuleBuilder<T, R> ruleBuilder, int minLength)
        {
            string msg = MessageUtils.GetMessage((int)ResponseCodeEnum.VLD_MIN_LENGTH, _langCode);

            return ruleBuilder.Custom((x, y) =>
            {
                if (x != null && !string.IsNullOrEmpty(x.ToString()) && x.ToString().Length < minLength)
                    y.AddFailure(y.DisplayName, string.Format(msg, minLength));
            });
        }

        public static IRuleBuilderOptionsConditions<T, R> RangeLength<T, R>(this IRuleBuilder<T, R> ruleBuilder, int minLength, int maxLength)
        {
            string msg = MessageUtils.GetMessage((int)ResponseCodeEnum.VLD_RANGE_LENGTH, _langCode);

            return ruleBuilder.Custom((x, y) =>
            {
                if (x != null && !string.IsNullOrEmpty(x.ToString()) && x.ToString().Length > minLength && x.ToString().Length < maxLength)
                    y.AddFailure(y.DisplayName, string.Format(msg, maxLength));
            });
        }

        public static IRuleBuilderOptionsConditions<T, R> NotNegative<T, R>(this IRuleBuilder<T, R> ruleBuilder)
        {
            string msg = MessageUtils.GetMessage((int)ResponseCodeEnum.VLD_NOT_NEGATIVE, _langCode);

            return ruleBuilder.Custom((x, y) =>
            {
                if (x != null && (!int.TryParse(x.ToString(), out int val) || val < 0))
                {
                    y.AddFailure(y.DisplayName, msg);
                }
            });
        }
        public static IRuleBuilderOptionsConditions<T, R> GreaterThanZero<T, R>(this IRuleBuilder<T, R> ruleBuilder)
        {
            string msg = MessageUtils.GetMessage((int)ResponseCodeEnum.VLD_GREATER_THAN_ZERO, _langCode);

            return ruleBuilder.Custom((value, context) =>
            {
                if (value != null && (!int.TryParse(value.ToString(), out int val) || val <= 0))
                {
                    context.AddFailure(context.DisplayName, msg);
                }
            });
        }
        public static IRuleBuilderOptionsConditions<T, R> MustBeInteger<T, R>(this IRuleBuilder<T, R> ruleBuilder)
        {
            string msg = MessageUtils.GetMessage((int)ResponseCodeEnum.VLD_MUST_BE_INTEGER, _langCode);

            return ruleBuilder.Custom((value, context) =>
            {
                if (value == null || !int.TryParse(value.ToString(), out _))
                {
                    context.AddFailure(context.DisplayName, msg);
                }
            });
        }
    }
}
