using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

public class SessionContextInterceptor : DbCommandInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionContextInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private void SetSessionContext(DbCommand command)
    {
        var userId = _httpContextAccessor.HttpContext?.Session.GetString("UserId");
        if (!string.IsNullOrEmpty(userId))
        {
            var sessionCommand = command.Connection.CreateCommand();
            sessionCommand.CommandText = "EXEC sp_set_session_context @key, @value";
            sessionCommand.Parameters.Add(new SqlParameter("@key", "UserId"));
            sessionCommand.Parameters.Add(new SqlParameter("@value", userId));
            sessionCommand.ExecuteNonQuery();
        }
    }

    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
    {
        SetSessionContext(command);
        return base.ReaderExecuting(command, eventData, result);
    }

    public override InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
    {
        SetSessionContext(command);
        return base.NonQueryExecuting(command, eventData, result);
    }

    public override InterceptionResult<object> ScalarExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<object> result)
    {
        SetSessionContext(command);
        return base.ScalarExecuting(command, eventData, result);
    }
}
