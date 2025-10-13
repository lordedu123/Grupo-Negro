using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Grupo_negro.Services
{
    public class CookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetCookie(string key, string value, int expireDays = 7)
        {
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(expireDays),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            _httpContextAccessor.HttpContext?.Response.Cookies.Append(key, value, options);
        }

        public string? GetCookie(string key)
        {
            return _httpContextAccessor.HttpContext?.Request.Cookies[key];
        }

        public void DeleteCookie(string key)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete(key);
        }

        public void SetObjectCookie<T>(string key, T value, int expireDays = 7)
        {
            var jsonValue = JsonSerializer.Serialize(value);
            SetCookie(key, jsonValue, expireDays);
        }

        public T? GetObjectCookie<T>(string key)
        {
            var cookieValue = GetCookie(key);
            if (string.IsNullOrEmpty(cookieValue))
                return default(T);

            try
            {
                return JsonSerializer.Deserialize<T>(cookieValue);
            }
            catch
            {
                return default(T);
            }
        }
    }
}