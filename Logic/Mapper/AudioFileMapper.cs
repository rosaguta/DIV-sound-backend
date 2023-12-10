using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
namespace Logic.Mapper
{
    public static class AudioFileMapper
    {
        public static Audiofile ConvertToLogic(this AudiofileDTO audiofileDTO)
        {
            if (audiofileDTO == null)
            {
                return new Audiofile();
            }
            return new Audiofile()
            {
                Id = audiofileDTO.Id,
                Filename = audiofileDTO.Filename,
                Duration = audiofileDTO.Duration,
                Path = audiofileDTO.Path,
                Uploaddate = audiofileDTO.Uploaddate,
                Uploaderid = audiofileDTO.Uploaderid,
                url = audiofileDTO.url

            };
        }
        public static AudiofileDTO ConvertToDTO (this Audiofile audiofile)
        {
            if (audiofile == null)
            {
                return new AudiofileDTO();
            }
            return new AudiofileDTO()
            {
                Id = audiofile.Id,
                Filename = audiofile.Filename,
                Duration = audiofile.Duration,
                Path = audiofile.Path,
                Uploaddate = audiofile.Uploaddate,
                Uploaderid = audiofile.Uploaderid,
                url = audiofile.url
            };
        }
    }
}
