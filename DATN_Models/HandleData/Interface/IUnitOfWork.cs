namespace DATN_Models.HandleData.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CommitAsync();
    }
}
