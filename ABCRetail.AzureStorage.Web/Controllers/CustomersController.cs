using Microsoft.AspNetCore.Mvc;
using ABCRetail.AzureStorage.Web.Services;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ABCRetail.AzureStorage.Web.Controllers
{
    public class CustomersController : Controller
    {
        private readonly TableStorageService _tableService;

        public CustomersController(TableStorageService tableService)
        {
            _tableService = tableService;
        }

        // GET: /Customers
        public async Task<IActionResult> Index()
        {
            var customers = new List<TableEntity>();
            await foreach (var entity in _tableService.CustomerTable.QueryAsync<TableEntity>())
            {
                customers.Add(entity);
            }
            return View(customers);
        }

        // GET: /Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string firstName, string lastName, string email)
        {
            var entity = new TableEntity("Customer", Guid.NewGuid().ToString())
            {
                { "FirstName", firstName },
                { "LastName", lastName },
                { "Email", email }
            };

            await _tableService.CustomerTable.AddEntityAsync(entity);

            return RedirectToAction(nameof(Index));
        }

        // GET: /Customers/Delete/{rowKey}
        public async Task<IActionResult> Delete(string rowKey)
        {
            if (!string.IsNullOrEmpty(rowKey))
            {
                await _tableService.CustomerTable.DeleteEntityAsync("Customer", rowKey);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

