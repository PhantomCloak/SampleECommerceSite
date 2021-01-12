namespace EFurni.Infrastructure.OutputDevices
{
    public interface IRepositoryQueryOutputDevice
    {
        int GetQueryResultCount();
        int GetQueryTotalCount();
        void SetQueryResultCount(int count);
        void SetQueryTotalCount(int count);

    }
}