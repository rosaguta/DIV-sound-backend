using DTO;
using Microsoft.AspNetCore.Http;

namespace DALInterface;

public interface IAudiofileDal
{
    string UploadFile(IFormFile file, AudiofileDTO audiofileDto);
    List<AudiofileDTO> GetFiles(int userid);
    bool DeleteFile(int audiofileid, int userid);
} 