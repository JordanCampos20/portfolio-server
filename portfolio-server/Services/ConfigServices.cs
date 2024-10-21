using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace finance_control_server.Services
{
    public static class ConfigServices
    {
        public static string GetId(IHttpContextAccessor _httpContextAccessor)
        {
            var idUsuario = _httpContextAccessor.HttpContext?.User
                            .Claims.FirstOrDefault(item => item.Type == ClaimTypes.NameIdentifier)?.Value;

            return idUsuario ?? "";
        }
        
        public static readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = false,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
}