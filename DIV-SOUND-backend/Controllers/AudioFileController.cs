using System.Net;
using Microsoft.AspNetCore.Mvc;
using Logic;
using Microsoft.Net.Http.Headers;

namespace DIV_SOUND_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class AudioFileController : ControllerBase
{
    [HttpPost]
    public string UploadFile(IFormFile formFile, int Uploaderid)
    {
        
        
        AudiofileCollection collection = new AudiofileCollection();
        string status = collection.UploadFile(formFile, Uploaderid);
        return status;
    }

    //[HttpGet]
    //public Audiofile GetFile()
    //{
    //    AudiofileCollection collection = new AudiofileCollection();
    //    Audiofile file = collection.GetFile();
    //    return file;
    //}

}