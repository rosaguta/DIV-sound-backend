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
    [Route("/AudioFiles")]
    public string UploadFile(IFormFile formFile, int Uploaderid)
    {
        
        
        AudiofileCollection collection = new AudiofileCollection();
        string status = collection.UploadFile(formFile, Uploaderid);
        return status;
    }

    [HttpGet]
    [Route("/AudioFiles/{userid}")]
    public List<Audiofile> GetFiles(int userid)
    {
        AudiofileCollection collection = new AudiofileCollection();
        collection.GetAudiofiles(userid);
        return collection.Audiofiles;
    }

    [HttpDelete]
    [Route("/AudioFiles")]
    public IActionResult DeleteFiles(int audiofileid, int userid)
    {
        bool deleted = new AudiofileCollection().deleteFile(audiofileid, userid);
        if (deleted)
        {
            return Ok();
        }
        return BadRequest();

    }
}