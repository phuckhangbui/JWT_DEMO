using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWT_DEMO.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SecureController : ControllerBase
    {
        [HttpGet("data")]
        public IActionResult GetData()
        {
            return Ok(new { Message = "This is a secure endpoint." });
        }

        [HttpGet("data/admin")]
        [Authorize(policy: "Admin")]
        public IActionResult GetDataAdmin()
        {
            return Ok(new { Message = "This is a secure endpoint. For Admin" });
        }

        [HttpGet("data/staff")]
        [Authorize(policy: "Staff")]
        public IActionResult GetDataStaff()
        {
            return Ok(new { Message = "This is a secure endpoint. For Staff" });
        }


        [HttpGet("data/member")]
        [Authorize(policy: "Member")]
        public IActionResult GetDataMember()
        {
            return Ok(new { Message = "This is a secure endpoint. For Member" });
        }

        [HttpGet("data/admin-and-staff")]
        [Authorize(policy: "AdminAndStaff")]
        public IActionResult GetDataAdminStaff()
        {
            return Ok(new { Message = "This is a secure endpoint. For Admin and Staff" });
        }
    }

}
