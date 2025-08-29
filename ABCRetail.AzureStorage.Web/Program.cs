using ABCRetail.AzureStorage.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Get Azure Storage connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("AzureStorage");

// Register TableStorageService as a singleton
builder.Services.AddSingleton(new TableStorageService(connectionString));

// Add MVC services
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton(new BlobStorageService(connectionString));

builder.Services.AddSingleton(new QueueStorageService(
    connectionString,
    builder.Configuration["Storage:QueueName"]
));



var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
