namespace EFurni.Infrastructure.OutputDevices
{
    public class RepositoryQueryOutputDeviceDevice : IRepositoryQueryOutputDevice
    {
        private int LastQueryResultCount { get; set; }
        private int LastQueryTotalCount { get; set; }
        
        public int GetQueryResultCount()
        {
            return LastQueryResultCount;
        }

        public int GetQueryTotalCount()
        {
            return LastQueryTotalCount;
        }

        public void SetQueryResultCount(int count)
        {
            LastQueryResultCount = count;
        }

        public void SetQueryTotalCount(int count)
        {
            LastQueryTotalCount = count;
        }
    }
}