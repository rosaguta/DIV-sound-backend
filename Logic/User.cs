using DAL;
using DALInterface;
using DTO;
using Logic.Mapper;
using System.Text;



namespace Logic;

public class User
{
    public int Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Mail { get; set; }
    public string Username { get; set; }
    public string Passhash { get; set; }
    IUserDal UserDal;
    public User()
    {
        UserDal = Factory.Factory.GetUserDal();
    }

    public User(string firstname, string lastname, string mail, string username, string passhash)
    {
        Firstname = firstname;
        Lastname = lastname;
        Mail = mail;
        Username = username;
        Passhash = passhash;
    }

    public User Try_GetUser(string uname)
    {
        User user = new User();
        UserDTO userDTO = UserDal.GetUser(uname);
        if (userDTO != null)
        {
            user = userDTO.ConvertToLogic();
        }
        else
        {
            user = null;
        }

        return user;
    }
    public bool CheckPassword(string Password, User User)
    {
        User user = new User();
        try
        {
            UserDTO userDTO = UserDal.GetUser(User.Username);
            user = userDTO.ConvertToLogic();
            string password = user.B64Decode();
            if (password != null)
            {
                if (password == Password)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    private string B64Decode()
    {
        byte[] data = Convert.FromBase64String(Passhash);
        string DecodedString = Encoding.UTF8.GetString(data);

        return DecodedString;
    }

}