using Azure.Data.Tables;

namespace ABCRetail.AzureStorage.Web.Services
{
    public class TableStorageService
    {
        private readonly TableServiceClient _serviceClient;
        public TableClient CustomerTable { get; }
        public TableClient ProductTable { get; }

        public TableStorageService(string connectionString)
        {
            _serviceClient = new TableServiceClient(connectionString);

            CustomerTable = _serviceClient.GetTableClient("Customers");
            ProductTable = _serviceClient.GetTableClient("Products");

            CustomerTable.CreateIfNotExists();
            ProductTable.CreateIfNotExists();
        }
    }
}
