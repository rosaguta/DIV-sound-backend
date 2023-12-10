using DTO;
using TagLib.IFD.Entries;

namespace Logic.Mapper;

public static class UserMapper
{
    public static User ConvertToLogic(this UserDTO userDto)
    {
        if (userDto == null)
        {
            return new User();
        }

        return new User()
        {
            Id = userDto.Id,
            Firstname = userDto.Firstname,
            Lastname = userDto.Firstname,
            Mail = userDto.Mail,
            Username = userDto.Username,
            Passhash = userDto.Passhash
        };
    }

    public static UserDTO ConvertToDTO(this User user)
    {
        if (user == null)
        {
            return new UserDTO();
        }

        return new UserDTO()
        {
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Mail = user.Mail,
            Username = user.Username,
            Passhash = user.Passhash,
        };
    }
}