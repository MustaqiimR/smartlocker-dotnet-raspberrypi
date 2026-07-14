using Microsoft.EntityFrameworkCore;
using SmartLocker.Web.Data;
using SmartLocker.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages();
builder.Services.AddControllers();

// Configure SQLite database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=smartlocker.db";
builder.Services.AddDbContext<SmartLockerDbContext>(options =>
    options.UseSqlite(connectionString));

// Add application services
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<BorrowLogService>();
builder.Services.AddScoped<BorrowService>();
builder.Services.AddScoped<RequestService>();
builder.Services.AddScoped<LockerService>();
builder.Services.AddScoped<LogService>();
builder.Services.AddScoped<QrCodeService>();
builder.Services.AddScoped<LockerHardwareService>();
builder.Services.AddScoped<ILockerHardwareService>(sp => sp.GetRequiredService<LockerHardwareService>());

// Add session support
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add authentication
builder.Services.AddAuthentication("SmartLockerAuth")
    .AddCookie("SmartLockerAuth", options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SmartLockerDbContext>();
    dbContext.Database.Migrate();
    DbInitializer.Initialize(dbContext);
}

app.MapRazorPages();
app.MapControllers();

app.Run();
