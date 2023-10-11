using DTO;
using Microsoft.AspNetCore.Http;

namespace DALInterface;

public interface IAudiofileDal
{
    string UploadFile(IFormFile file, AudiofileDTO audiofileDto);
    void StoreTempFile(IFormFile file);
    void RemoveTempFile(string remotefilepath);
}