using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace HotelAndTours.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileImageController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
    {
        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var path = Path.Combine(Directory.GetCurrentDirectory(), "images/" + fileName);
        var stream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(stream);
        stream.Close();
        return Created("", new { FileName = fileName });
    }

    [HttpGet("images/{imageName}")]
    public IActionResult GetImage(string imageName)
    {
        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "images", imageName);

        if (!System.IO.File.Exists(imagePath)) return NotFound();

        // Read the image file as bytes
        var imageBytes = System.IO.File.ReadAllBytes(imagePath);

        // Determine the content type based on the image file extension
        var contentType = GetContentType(imagePath);

        // Return the image file as the response with the appropriate content type
        return File(imageBytes, contentType);
    }

    private string GetContentType(string path)
    {
        var provider = new FileExtensionContentTypeProvider();
        string contentType;
        if (!provider.TryGetContentType(path, out contentType)) contentType = "application/octet-stream";
        return contentType;
    }
}