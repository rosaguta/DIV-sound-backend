using System.Windows.Markup;
using DAL;
using DALInterface;
using DTO;
using Logic.Mapper;
using Factory;

namespace Logic;

public class UserCollection
{
    public List<User> Userlist { get;  set; }
    IUserDal UserDal;

    public UserCollection()
    {
        Userlist = new List<User>();
        UserDal = Factory.Factory.GetUserDal();
    }

    public bool Register(string firstname, string lastname, string password, string mail, string username)
    {
        DTO.UserDTO userDTO = new UserDTO();
        UserDTO? userexists = UserDal.GetUser(username);
        if (userexists == null)
        {
            object passhash = EncodePasswordToBase64(password);
            User user = new User(firstname, lastname, mail, username, passhash.ToString());
            UserDTO userDto = user.ConvertToDTO();
            bool created = UserDal.NewUser(userDto);
            return created;
        }

        return false;
    }

    public List<User> GetUsers()
    {
        List<UserDTO> userdtolist = UserDal.GetAllUsers();
        UserCollection usercollection = new UserCollection();
        foreach (var UserDTO in userdtolist)
        {
            usercollection.Userlist.Add(UserDTO.ConvertToLogic());
        }

        return usercollection.Userlist;
    }

    public User GetUser(string username)
    {
        UserDTO userdto = UserDal.GetUser(username);
        User user = userdto.ConvertToLogic();
        return user;
    }

    public List<string> GetAudioFiles(int id)
    {
        List<string> urls = UserDal.GetUserAudioUrls(id);
        return urls;
    }
    private object EncodePasswordToBase64(string password)
    {
        if (password != null)
        {
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            password = Convert.ToBase64String(passwordBytes);
            return password.ToString();
        }
        return password;
    }
    
}