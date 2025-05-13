using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace BirSiberDanismanlik.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CacheTestController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CacheTestController(IMemoryCache memoryCache, IDistributedCache distributedCache, IHttpContextAccessor httpContextAccessor)
        {
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
            _httpContextAccessor = httpContextAccessor;
        }

        private IActionResult AccessDeniedIfNotAdmin()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity.IsAuthenticated || !user.IsInRole("Admin"))
            {
                Response.StatusCode = 403;
                return Content(
                    "<html><body style='background:#181818;color:#fff;text-align:center;padding:60px;'><h2>Erisim Reddedildi</h2><p>Bu sayfaya erisim yetkiniz yok.</p><a href='/' style='color:#6cf;'>Ana Sayfa</a></body></html>",
                    "text/html"
                );
            }
            return null;
        }

        [HttpGet("memory/{key}")]
        public IActionResult TestMemoryCache(string key)
        {
            var denied = AccessDeniedIfNotAdmin();
            if (denied != null) return denied;
            bool fromCache = _memoryCache.TryGetValue(key, out string value);
            if (!fromCache)
            {
                value = $"MemoryCache: {key} - {DateTime.Now:HH:mm:ss}";
                _memoryCache.Set(key, value, TimeSpan.FromMinutes(2));
            }
            var html = $@"
                <html>
                <head>
                    <title>MemoryCache Test</title>
                    <style>
                        body {{ background: #181818; color: #eee; font-family: Arial, sans-serif; padding: 40px; }}
                        .container {{ background: #232323; border-radius: 8px; padding: 32px; max-width: 500px; margin: auto; box-shadow: 0 2px 8px #0004; }}
                        h2 {{ color: #6cf; }}
                        .from-cache {{ color: {(fromCache ? "#0f0" : "#f66")}; font-weight: bold; }}
                        .back-btn {{ background: #444; color: #fff; border: none; border-radius: 4px; padding: 8px 18px; margin-top: 24px; cursor: pointer; font-size: 1rem; transition: background 0.2s; }}
                        .back-btn:hover {{ background: #6cf; color: #222; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h2>MemoryCache Test</h2>
                        <p><b>Key:</b> {key}</p>
                        <p><b>Value:</b> {value}</p>
                        <p><b>From Cache:</b> <span class='from-cache'>{fromCache}</span></p>
                        <a href='/' class='back-btn'>&larr; Back</a>
                    </div>
                </body>
                </html>
            ";
            return Content(html, "text/html");
        }

        [HttpGet("redis/{key}")]
        public async Task<IActionResult> TestRedisCache(string key)
        {
            var denied = AccessDeniedIfNotAdmin();
            if (denied != null) return denied;
            var value = await _distributedCache.GetStringAsync(key);
            bool fromCache = !string.IsNullOrEmpty(value);
            if (!fromCache)
            {
                value = $"RedisCache: {key} - {DateTime.Now:HH:mm:ss}";
                await _distributedCache.SetStringAsync(key, value, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
                });
            }
            var html = $@"
                <html>
                <head>
                    <title>RedisCache Test</title>
                    <style>
                        body {{ background: #181818; color: #eee; font-family: Arial, sans-serif; padding: 40px; }}
                        .container {{ background: #232323; border-radius: 8px; padding: 32px; max-width: 500px; margin: auto; box-shadow: 0 2px 8px #0004; }}
                        h2 {{ color: #fc6; }}
                        .from-cache {{ color: {(fromCache ? "#0f0" : "#f66")}; font-weight: bold; }}
                        .back-btn {{ background: #444; color: #fff; border: none; border-radius: 4px; padding: 8px 18px; margin-top: 24px; cursor: pointer; font-size: 1rem; transition: background 0.2s; }}
                        .back-btn:hover {{ background: #fc6; color: #222; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h2>RedisCache Test</h2>
                        <p><b>Key:</b> {key}</p>
                        <p><b>Value:</b> {value}</p>
                        <p><b>From Cache:</b> <span class='from-cache'>{fromCache}</span></p>
                        <a href='/' class='back-btn'>&larr; Back</a>
                    </div>
                </body>
                </html>
            ";
            return Content(html, "text/html");
        }

        [HttpGet("complex/{key}")]
        public IActionResult TestComplexCache(string key)
        {
            var exampleObj = new
            {
                id = 42,
                user = new {
                    id = "abc-123",
                    name = "Yasin Seyhun",
                    email = "yasin@example.com"
                },
                appointment = new {
                    date = DateTime.Now.AddDays(1),
                    service = "Sızma Testleri (Pentest)",
                    status = "Beklemede"
                },
                tags = new[] { "cache", "test", "json" }
            };

            if (!_memoryCache.TryGetValue(key, out string json))
            {
                json = JsonSerializer.Serialize(exampleObj, new JsonSerializerOptions { WriteIndented = true });
                _memoryCache.Set(key, json, TimeSpan.FromMinutes(2));
            }
            return Content(json, "application/json");
        }

        [HttpGet("complex-redis/{key}")]
        public async Task<IActionResult> TestComplexRedisCache(string key)
        {
            var exampleObj = new
            {
                id = 42,
                user = new {
                    id = "abc-123",
                    name = "Yasin Seyhun",
                    email = "yasin@example.com"
                },
                appointment = new {
                    date = DateTime.Now.AddDays(1),
                    service = "Sızma Testleri (Pentest)",
                    status = "Beklemede"
                },
                tags = new[] { "cache", "test", "json" }
            };

            var json = await _distributedCache.GetStringAsync(key);
            if (string.IsNullOrEmpty(json))
            {
                json = JsonSerializer.Serialize(exampleObj, new JsonSerializerOptions { WriteIndented = true });
                await _distributedCache.SetStringAsync(key, json, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
                });
            }
            return Content(json, "application/json");
        }
    }
} 