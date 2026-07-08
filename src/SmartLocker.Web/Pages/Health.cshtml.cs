using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartLocker.Web.Data;
using System.Text.Json;

namespace SmartLocker.Web.Pages
{
    [IgnoreAntiforgeryToken]
    public class HealthModel : PageModel
    {
        private readonly SmartLockerDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<HealthModel> _logger;

        public HealthModel(SmartLockerDbContext context, IConfiguration configuration, ILogger<HealthModel> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            try
            {
                var health = new
                {
                    status = "healthy",
                    timestamp = DateTime.UtcNow,
                    version = "1.0.0",
                    environment = _configuration["SmartLocker:Environment"] ?? "Unknown",
                    database = CheckDatabase(),
                    hardware = new
                    {
                        mode = _configuration["LockerHardware:Mode"] ?? "Mock",
                        gpioEnabled = _configuration.GetValue<bool>("SmartLocker:Hardware:GpioEnabled")
                    }
                };

                return new JsonResult(health);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Health check failed: {ex.Message}");
                
                var unhealthy = new
                {
                    status = "unhealthy",
                    timestamp = DateTime.UtcNow,
                    error = ex.Message
                };

                return new JsonResult(unhealthy) { StatusCode = 503 };
            }
        }

        private object CheckDatabase()
        {
            try
            {
                // Try to execute a simple query
                var count = _context.Users.Count();
                return new { status = "connected", users = count };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Database check failed: {ex.Message}");
                return new { status = "disconnected", error = ex.Message };
            }
        }
    }
}
