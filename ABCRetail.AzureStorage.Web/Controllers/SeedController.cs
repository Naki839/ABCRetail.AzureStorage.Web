using ABCRetail.AzureStorage.Web.Services;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ABCRetail.AzureStorage.Web.Controllers
{
    public class SeedController : Controller
    {
        private readonly TableStorageService _tableService;

        public SeedController(TableStorageService tableService)
        {
            _tableService = tableService;
        }

        public async Task<IActionResult> Index()
        {
            // Sample customers
            var customer1 = new TableEntity("Customer", Guid.NewGuid().ToString())
            {
                { "FirstName", "John" },
                { "LastName", "Doe" },
                { "Email", "john@example.com" }
            };

            var customer2 = new TableEntity("Customer", Guid.NewGuid().ToString())
            {
                { "FirstName", "Jane" },
                { "LastName", "Smith" },
                { "Email", "jane@example.com" }
            };

            await _tableService.CustomerTable.AddEntityAsync(customer1);
            await _tableService.CustomerTable.AddEntityAsync(customer2);

            // Sample products
            var product1 = new TableEntity("Product", Guid.NewGuid().ToString())
            {
                { "Name", "Laptop" },
                { "Description", "Gaming Laptop" },
                { "Price", 1200 }
            };

            var product2 = new TableEntity("Product", Guid.NewGuid().ToString())
            {
                { "Name", "Phone" },
                { "Description", "Smartphone" },
                { "Price", 800 }
            };

            await _tableService.ProductTable.AddEntityAsync(product1);
            await _tableService.ProductTable.AddEntityAsync(product2);

            return Content("Seeding complete! Check your Azure Tables.");
        }
    }
}


