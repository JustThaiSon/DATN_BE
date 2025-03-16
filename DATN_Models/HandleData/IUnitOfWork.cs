using DATN_Models.HandleData.Interface;

namespace DATN_Models.HandleData
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DATN_Context context;


        public UnitOfWork(DATN_Context context)
        {
            this.context = context;
        }
        public async Task<int> CommitAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(true);
        }
    }
}
