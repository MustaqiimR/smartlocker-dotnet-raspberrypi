using System.Text;

namespace SmartLocker.Web.Services
{
    public class QrCodeService
    {
        private readonly IConfiguration _configuration;

        public QrCodeService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Generates a QR code URL for the given token
        /// </summary>
        public string GenerateQrCodeUrl(string token)
        {
            var baseUrl = GetBaseUrl();
            return $"{baseUrl}/unlock/{token}";
        }

        /// <summary>
        /// Generates a simple SVG QR code representation
        /// For production, consider using a library like QRCoder or ZXing.Net
        /// </summary>
        public string GenerateQrCodeSvg(string token)
        {
            var url = GenerateQrCodeUrl(token);
            
            // Simple placeholder SVG with the URL encoded as text
            // In production, this should use a proper QR code library
            var svg = $@"
<svg xmlns='http://www.w3.org/2000/svg' width='200' height='200' viewBox='0 0 200 200'>
  <rect width='200' height='200' fill='white'/>
  <text x='100' y='100' font-size='10' text-anchor='middle' dominant-baseline='middle'>
    <tspan x='100' dy='0'>QR Code</tspan>
    <tspan x='100' dy='15'>Token: {token.Substring(0, 8)}...</tspan>
  </text>
  <text x='100' y='180' font-size='8' text-anchor='middle' fill='#666'>
    {url}
  </text>
</svg>";

            return svg;
        }

        /// <summary>
        /// Generates a data URI for embedding QR code directly in HTML
        /// </summary>
        public string GenerateQrCodeDataUri(string token)
        {
            var svg = GenerateQrCodeSvg(token);
            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(svg));
            return $"data:image/svg+xml;base64,{base64}";
        }

        /// <summary>
        /// Gets the base URL for the application
        /// </summary>
        private string GetBaseUrl()
        {
            var baseUrl = _configuration["SmartLocker:BaseUrl"];
            
            if (string.IsNullOrEmpty(baseUrl))
            {
                // Default to localhost if not configured
                baseUrl = "http://localhost:5000";
            }

            return baseUrl.TrimEnd('/');
        }
    }
}
