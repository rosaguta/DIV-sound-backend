using System.Net;
using Microsoft.AspNetCore.Mvc;
using Logic;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.Net.Http.Headers;

namespace DIV_SOUND_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class AudioFileController : ControllerBase
{
    [HttpPost]
    [Route("/AudioFiles")]
    public string UploadFile(List<IFormFile> formFiles, int Uploaderid)
    {

        AudiofileCollection collection = new AudiofileCollection();
        string status = "";
        foreach (IFormFile formFile in formFiles)
        {
            if(formFile.ContentType == "audio/mpeg"){
                status = collection.UploadFile(formFile, Uploaderid);
            }
        }   
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