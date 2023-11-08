using System.Reflection;

namespace DALTest;
using DALInterface;
using Logic;
using DTO;

public class UserDal : IUserDal
{
    public List<UserDTO> GetAllUsers()
    {
        List<UserDTO> users = new List<UserDTO>();
        users.Add(new UserDTO(0,"Rose", "van Leeuwen", "Rose@mail.com", "Rose", "c3RyaW5n"));
        users.Add(new UserDTO(1,"Liv", "Knapen", "Liv@mail.com", "Liv", "c3RyaW5n"));
        return users;
    }

    public UserDTO GetUser(string email)
    {
        if (email == "Rose@mail.com")
        {
            return new UserDTO(0,"Rose", "van Leeuwen", "Rose@mail.com", "Rose", "c3RyaW5n");
        }
        return null;
    }

    public bool NewUser(UserDTO user)
    {
        Type objectType = user.GetType();
        PropertyInfo[] properties = objectType.GetProperties();

        foreach (PropertyInfo property in properties)
        {
            object value = property.GetValue(user);

            // Check if the property value is null or empty
            if (value == null || (value is string && string.IsNullOrEmpty((string)value)))
            {
                return false;
            }
        }

        return true;
    }

    public List<string> GetUserAudioUrls(int id)
    {
        if (id == 1)
        {
            List<string> urllist = new List<string>();
            urllist.Add("http://138.201.52.251:9998/3/big bottom.mp3");
            urllist.Add("http://138.201.52.251:9998/3/congrats.mp3");
            return urllist;
        }

        return null;
    }
}