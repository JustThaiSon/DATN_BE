using DATN_Helpers.Extensions;
using DATN_Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;

public class ChangeLogInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
      DbContextEventData eventData,
      InterceptionResult<int> result,
      CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context == null) return base.SavingChangesAsync(eventData, result, cancellationToken);

        var changeLogs = new List<ChangeLog>();
        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
            {
                var changeLog = new ChangeLog
                {
                    TableName = entry.Entity.GetType().Name,
                    Action = entry.State.ToString(),
                    RecordId = Guid.NewGuid(),
                    ChangeDateTime = DateTime.Now,
                    UserId = HttpContextHelper.GetUserId()
                };

                if (entry.State == EntityState.Modified)
                {
                    changeLog.BeforeChange = JsonSerializer.Serialize(entry.OriginalValues.ToObject());
                    changeLog.AfterChange = JsonSerializer.Serialize(entry.CurrentValues.ToObject());
                }
                else if (entry.State == EntityState.Added)
                {
                    changeLog.AfterChange = JsonSerializer.Serialize(entry.CurrentValues.ToObject());
                }
                else if (entry.State == EntityState.Deleted)
                {
                    changeLog.BeforeChange = JsonSerializer.Serialize(entry.OriginalValues.ToObject());
                }

                changeLogs.Add(changeLog);
            }
        }

        if (changeLogs.Any())
        {
            context.Set<ChangeLog>().AddRange(changeLogs);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
