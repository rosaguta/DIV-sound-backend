using System.Net;
using DALInterface;
using Microsoft.AspNetCore.Http;
using DALInterface;
using DTO;
using FFmpeg.AutoGen;
using Google.Protobuf.WellKnownTypes;
using Logic.Mapper;
using NLayer.NAudioSupport;
using TagLib;
using TagLib.Mpeg;

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

    public string UploadFile(IFormFile file, int uploaderid)
    {
        
        AudiofileDTO audioFileDto = new AudiofileDTO();
        audioFileDto.Filename = file.FileName;
        audioFileDto.Uploaddate = DateTime.Now;
        audioFileDto.Duration = null;
        audioFileDto.Uploaderid = uploaderid;
        string status = audiofileDal.UploadFile(file, audioFileDto);
        return status;
    }


    



}