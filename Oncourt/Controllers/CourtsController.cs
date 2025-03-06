using Microsoft.AspNetCore.Mvc;

namespace OnCourt.Controllers
{
    [ApiController]
    [Route("api/courts")] // URL: https://localhost:7117/api/courts
    public class CourtsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetCourts()
        {
            var courts = new[]
            {
                new { Id = 1, Name = "Central Park Tennis", Location = "New York, NY" },
                new { Id = 2, Name = "Edmonton Tennis Center", Location = "Edmonton, AB" }
            };

            return Ok(courts);
        }
    }
}
