using System.Configuration;

namespace CoursesManager.UI.Repositories;

public abstract class BaseRepository
{
    protected readonly int _dbDataInvalidationSeconds = int.Parse(ConfigurationManager.AppSettings["DbDataInvalidationSeconds"] ?? throw new InvalidOperationException());
    protected DateTime _lastUpdated = DateTime.Now;

    protected bool ShouldRefresh => DateTime.Now.AddSeconds(-_dbDataInvalidationSeconds) < _lastUpdated;
}