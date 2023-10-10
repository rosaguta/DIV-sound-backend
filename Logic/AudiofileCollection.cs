using System.Net;
using DALInterface;
using Microsoft.AspNetCore.Http;

namespace Logic;

public class AudiofileCollection
{
    public List<Audiofile> Audiofiles { get; set; }
    IAudiofileDal audiofileDal;
    
    public AudiofileCollection()
    {
        Audiofiles = new List<Audiofile>();
        audiofileDal =  Factory.Factory.GetAudiofileDal();
    }

    public string UploadFile(IFormFile file)
    {
        string status = audiofileDal.UploadFile(file);
        return status;
    }
    
   

}