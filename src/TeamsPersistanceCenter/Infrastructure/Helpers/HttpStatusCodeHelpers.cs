using System.Net;

namespace TeamsPersistanceCenter.Api.Infrastructure.Helpers
{
    public static class HttpStatusCodeHelpers
    {
        public static bool IsSuccessStatusCode(int? statusCode) => statusCode >= 200 && statusCode < 300;

        public static bool IsSuccessStatusCode(this HttpStatusCode statusCode) => IsSuccessStatusCode((int)statusCode);
    }
}
