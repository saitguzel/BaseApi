using Base.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Base.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        protected readonly IDBService _IDBService;

        public UserController(IDBService IDBService)
        {
            _IDBService = IDBService;
        }

        [HttpGet]
        [Route("GetUserList")]
        public async Task<IActionResult> GetUserList()
        {
            return Ok(await _IDBService.GetUserList());
        }

    }
}
