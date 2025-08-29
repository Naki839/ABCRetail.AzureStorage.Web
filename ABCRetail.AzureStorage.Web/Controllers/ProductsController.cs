using Microsoft.AspNetCore.Mvc;
using ABCRetail.AzureStorage.Web.Services;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ABCRetail.AzureStorage.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly TableStorageService _tableService;
        private readonly BlobStorageService _blobService;
        private readonly QueueStorageService _queueService;

        // Constructor
        public ProductsController(
            TableStorageService tableService,
            BlobStorageService blobService,
            QueueStorageService queueService)
        {
            _tableService = tableService;
            _blobService = blobService;
            _queueService = queueService;
        }

        // GET: /Products
        public async Task<IActionResult> Index()
        {
            var products = new List<TableEntity>();
            await foreach (var entity in _tableService.ProductTable.QueryAsync<TableEntity>())
            {
                products.Add(entity);
            }
            return View(products);
        }

        // GET: /Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name, string description, decimal price, IFormFile image)
        {
            string imageUrl = null;

            // Upload image to Blob Storage and send queue message
            if (image != null)
            {
                imageUrl = await _blobService.UploadFileAsync(image);
                await _queueService.SendMessageAsync($"Uploaded image: {image.FileName}");
            }

            // Create TableEntity for the product
            var entity = new TableEntity("Product", Guid.NewGuid().ToString())
            {
                { "Name", name },
                { "Description", description },
                { "Price", price },
                { "ImageUrl", imageUrl ?? "" }
            };

            // Add to Azure Table Storage and send queue message
            await _tableService.ProductTable.AddEntityAsync(entity);
            await _queueService.SendMessageAsync($"Processed order for product: {name}");

            return RedirectToAction(nameof(Index));
        }

        // GET: /Products/Delete/{rowKey}
        public async Task<IActionResult> Delete(string rowKey)
        {
            if (!string.IsNullOrEmpty(rowKey))
            {
                await _tableService.ProductTable.DeleteEntityAsync("Product", rowKey);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
