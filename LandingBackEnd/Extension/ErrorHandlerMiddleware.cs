using DATN_Helpers.Common;
using DATN_Helpers.Constants;
using DATN_Helpers.Extensions;
using DATN_Models.HandleData.Interface;
using System.Net;
using System.Text.Json;
using ValidationException = FluentValidation.ValidationException;
namespace DATN_LandingPage.Extension
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly string _langCode;

        public ErrorHandlerMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _langCode = configuration["ProjectSettings:LanguageCode"] ?? "vi";

            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException vex)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.BadRequest;

                var res = new CommonResponse<object>();
                res.ResponseCode = (int)ResponseCodeEnum.ERR_WRONG_INPUT;
                res.Message = string.Join(",", vex.Errors.Select(failure => failure.ErrorMessage));

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var result = JsonSerializer.Serialize(res, options);
                await response.WriteAsync(result);

            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                // Write log exception
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var logger = scope.ServiceProvider.GetService<ILogger<ErrorHandlerMiddleware>>();

                    logger.LogError(error, "Unhandled exception occurred");
                }

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();

                    var res = new CommonResponse<object>();
                    res.ResponseCode = (int)ResponseCodeEnum.ERR_SYSTEM;
                    res.Message = MessageUtils.GetMessage(res.ResponseCode, _langCode);

                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };
                    var result = JsonSerializer.Serialize(res, options);
                    await response.WriteAsync(result);
                }
            }
        }
    }
}
