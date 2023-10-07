using DTO;

namespace DALInterface;

public interface IAudiofileDal
{
    void UploadFile(AudiofileDTO file);
}