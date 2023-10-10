using System.Net;
using Microsoft.AspNetCore.Mvc;
using Logic;

namespace DIV_SOUND_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class AudioFileController : ControllerBase
{
    [HttpPost]
    public string UploadFile(IFormFile formFile)
    {
        AudiofileCollection collection = new AudiofileCollection();
        collection.
    }

    [HttpGet]
    public Audiofile GetFile()
    {
        AudiofileCollection collection = new AudiofileCollection();
        Audiofile file = collection.GetFile();
        return file;
    }

}