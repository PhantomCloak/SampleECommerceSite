namespace EFurni.Service.OutputDevices
{
    public class ProductServiceOutputDeviceDevice : IProductServiceOutputDevice
    {
        private (int,int) LastFilteredProductCount { get; set; }
        public (int FilteredCount, int TotalCount) GetFilteredProductCount()
        {
            return LastFilteredProductCount;
        }

        public void SetQueryResultCount(int filtered, int remain)
        {
            LastFilteredProductCount = (filtered, remain);
        }
    }
}