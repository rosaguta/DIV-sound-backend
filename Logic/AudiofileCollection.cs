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
using NAudio.Lame;
using NAudio.Wave;

namespace Logic;

public class AudiofileCollection
{
    public List<Audiofile> Audiofiles { get; set; }
    IAudiofileDal audiofileDal;

    public AudiofileCollection()
    {
        Audiofiles = new List<Audiofile>();
        audiofileDal = Factory.Factory.GetAudiofileDal();
    }

    public string UploadFile(IFormFile file, int uploaderid)
    {
        if(file.ContentType == "audio/mpeg")
        {
            GetAudioDuration(file);
            AudiofileDTO audioFileDto = new AudiofileDTO();
            audioFileDto.Filename = file.FileName;
            audioFileDto.Uploaddate = DateTime.Now;
            audioFileDto.Duration = null;
            audioFileDto.Uploaderid = uploaderid;
            string status = audiofileDal.UploadFile(file, audioFileDto);
            return status;
        }
        else
        {
            return "invalid file";
        }
        
    }

    public List<Audiofile> GetAudiofiles(int userid)
    {
        List<AudiofileDTO> audiofileDTOs = audiofileDal.GetFiles(userid);
        foreach (var file in audiofileDTOs)
        {
            Audiofiles.Add(file.ConvertToLogic());
        }
        return Audiofiles;
    }

    public bool deleteFile(int audiofileid, int userid)
    {
        List<AudiofileDTO> audiofileDtos = audiofileDal.GetFiles(userid);
        AudiofileDTO filedto = audiofileDtos.FirstOrDefault(obj => obj.Id == audiofileid);
        bool deleted = audiofileDal.DeleteFile(audiofileid, userid, filedto.Path);
        return deleted;
    }

    private double? GetAudioDuration(IFormFile mp3File)
    {
        if (mp3File != null && mp3File.Length > 0)
        {
            using (var mp3Stream = mp3File.OpenReadStream())
            {
                using (var mp3Reader = new Mp3FileReader(mp3Stream))
                {
                    return mp3Reader.TotalTime.TotalSeconds;
                }
            }
        }

        return 0; // or handle error accordingly
    }
}