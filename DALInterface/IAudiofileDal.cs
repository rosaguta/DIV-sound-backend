using DTO;
using Microsoft.AspNetCore.Http;

namespace DALInterface;

public interface IAudiofileDal
{
    string UploadFile(IFormFile file, AudiofileDTO audiofileDto);
    List<AudiofileDTO> GetFiles(int userid);
    //List<AudiofileDTO> GetFiles(int userid, int boardid);
} 