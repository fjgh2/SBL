using Microsoft.AspNetCore.Mvc;

namespace SBL.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MobileClientController : ControllerBase
{
    [HttpGet]
    [Route("apk")]
    public ActionResult GetApkAsync()
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles", "app.apk");

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("APK file not found");
        }

        string contentType = "application/vnd.android.package-archive";
        string fileName = "yourapp.apk";

        return PhysicalFile(filePath, contentType, fileName);
    }
}
