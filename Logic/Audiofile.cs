using DALInterface;
using DTO;
using Logic.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace Logic;

public class Audiofile
{
    public string filename { get; private set; }
    public float duration { get; private set; }
    public string filetype { get; private set; }
    public string path { get; private set; }
    public IFormFile file { get; private set; }

    public Audiofile()
    {
    }

    // public void UploadFile(AudiofileDTO audiofile)
    // {
    //     MetadataHandler.HandleMetadata(audiofile.path);
    //     
    // }
}