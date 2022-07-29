using Microsoft.AspNetCore.Mvc;

namespace PictureStorage.Controllers;

[ApiController]
[Route("[controller]")]
public class PictureController : ControllerBase
{
    private const string StoragePath = "Storage/Pictures/";
    
    [HttpPost]
    public async Task<IActionResult> UploadImage()
    {
        List<(string, bool)> filesAdded = new();
        foreach (IFormFile formFile in Request.Form.Files)
        {
            
            var fileName = GetFileName(formFile);
            var path = $"Storage/Pictures/{fileName}";
            try
            {
                await using (var filestream = System.IO.File.Create(path))
                {
                    await formFile.CopyToAsync(filestream);
                    filesAdded.Add((fileName, true));
                }
            }
            catch (Exception)
            {
                filesAdded.Add((fileName, false));
            }
        }

        return Ok(filesAdded.Select(tuple => new
        {
            name = tuple.Item1,
            success = tuple.Item2
        }));
    }

    private static string GetFileName(IFormFile formFile)
    {
        var originalFilenameParts = formFile.FileName.Split('.');
        var fileExtension = originalFilenameParts.Last();
        var originalFileName = string.Join(null, originalFilenameParts.Take(originalFilenameParts.Length - 1));
        var fileName = originalFileName + Guid.NewGuid() + '.' + fileExtension;
        return fileName;
    }

    [HttpGet]
    public FileStreamResult GetImage(string name)
    {
        FileStream? result = null;
        try
        {
            result = System.IO.File.Open($"{StoragePath}{name}", FileMode.Open);
        }
        finally
        {
            if (result == null) throw new ApplicationException("No File with the given Name has been found");
        }

        return new FileStreamResult(result, $"image/{name.Trim().Split('.').Last()}");
    }
}