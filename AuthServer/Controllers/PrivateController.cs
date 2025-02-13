using AuthServer.CustomAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using API;
// using ToDoDbContext.ToDoDbContext;
using Microsoft.Extensions.Logging;

namespace AuthServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PrivateController : AuthenticatedController
    {
        private readonly ToDoDbContext _dataContext;
        private readonly ILogger<PrivateController> _logger;

        public PrivateController(ToDoDbContext dataContext,ILogger<PrivateController> logger)
        {
            _dataContext = dataContext;
             _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Session>>> Get()
        {    _logger.LogInformation("Entering Get method");
            // ודא שה-Identity כבר אוחסן ב-AuthenticatedController על ידי הפילטר
            var userId = Identity?.Id;  // השתמש ב-Id של המשתמש מתוך Identity
              _logger.LogWarning("No sessions found for User ID: {UserId}", userId);
            if (!userId.HasValue)
            {
                _logger.LogWarning("User ID not found. Returning Unauthorized.");
                
                return Unauthorized();  // אם אין משתמש מחובר, תחזור עם תשובת שגיאה
            }

            // שליפת הסשנים המתאימים למשתמש המחובר
            var sessions = await _dataContext.Sessions
                .Where(s => s.UserId == userId.Value)  // שליפת הסשנים לפי UserId
                .OrderByDescending(s => s.Date)  // מיון הסשנים לפי תאריך
                .ToListAsync();  // ביצוע השאילתה

            return Ok(sessions);  // החזרת הסשנים
        }
    }
}
