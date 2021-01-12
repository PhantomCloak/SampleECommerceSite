namespace EFurni.Service.OutputDevices
{
    public interface IProductServiceOutputDevice
    {
        (int FilteredCount,int TotalCount) GetFilteredProductCount();
        void SetQueryResultCount(int filtered,int remain);
    }
}