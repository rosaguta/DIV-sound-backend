using DTO;

namespace DALInterface;

public interface IUserDal
{
    UserDTO GetUser(string username);
    bool NewUser(UserDTO userDTO);
    List<UserDTO> GetAllUsers();
    List<string> GetUserAudioUrls(int id);
}