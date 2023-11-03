using System.Data;
using DALInterface;
using DTO;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
namespace DAL;

public class UserDal : IUserDal
{
    private string? Getconnectionstring()
    {
        string? jsonfile = "appsettings.json";
        JsonObject? jsonObject = (JsonObject?)JsonObject.Parse(File.ReadAllText(jsonfile));
        string? sqlservervalue = (string?)jsonObject["ConnectionStrings"]["SqlServer"];
        return sqlservervalue;
    }
    
    public UserDTO? GetUser(string uname)
    {
        string? connectionstring = Getconnectionstring();
        MySqlConnection conn = new MySqlConnection(connectionstring);
        conn.Open();
        //conn.OpenAsync().Wait();
        string query = "SELECT * FROM user WHERE username = @username";
        MySqlCommand cmd = new MySqlCommand(query, conn);
        DataTable dataTable = new DataTable();
        cmd.Parameters.AddWithValue("@username", uname);
        var data = cmd.ExecuteReader();
        if (data.Read())
        {
            UserDTO userdto = new UserDTO()
            {
                Id = Convert.ToInt32(data["id"]),
                Firstname = Convert.ToString(data["firstname"]),
                Lastname = Convert.ToString(data["lastname"]),
                Mail = Convert.ToString(data["mail"]),
                Passhash = Convert.ToString(data["passhash"]),
                Username = Convert.ToString(data["username"])
            };
            conn.Close();
            return userdto;
        }
        conn.Close();
        return null;
    }

    public bool NewUser(UserDTO userDto)
    {
        string? connectionstring = Getconnectionstring();
        MySqlConnection conn = new MySqlConnection(connectionstring);
        conn.Open();
        string query = "INSERT INTO user (firstname, lastname, username, mail, passhash)" +
                       "VALUES(@Firstname, @Lastname, @Username, @Mail, @Passhash)";
        using (MySqlCommand cmd = new MySqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@Firstname", userDto.Firstname);
            cmd.Parameters.AddWithValue("@Lastname", userDto.Lastname);
            cmd.Parameters.AddWithValue("@Username", userDto.Username);
            cmd.Parameters.AddWithValue("@Mail", userDto.Mail);
            cmd.Parameters.AddWithValue("@Passhash",userDto.Passhash);
            int rowsaffected = cmd.ExecuteNonQuery();
            conn.Close();
            return rowsaffected > 0;
        }
    }

    public List<UserDTO> GetAllUsers()
    {
        List<UserDTO> userdtolist = new List<UserDTO>();
        string? connectionstring = Getconnectionstring();
        MySqlConnection conn = new MySqlConnection(connectionstring);
        conn.Open();
        string query = "SELECT * FROM user";
        MySqlCommand command = new MySqlCommand(query, conn);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            UserDTO userdto = new UserDTO();
            userdto.Id = reader.GetInt32("id");
            userdto.Firstname = reader.GetString("firstname");
            userdto.Lastname = reader.GetString("lastname");
            userdto.Username = reader.GetString("username");
            userdto.Mail = reader.GetString("mail");
            userdto.Passhash = reader.GetString("passhash");
            userdtolist.Add(userdto);
        }
        conn.Close();
        return userdtolist;
    }

    public List<string> GetUserAudioUrls(int id)
    {
        List<string> urls = new List<string>();
        string? connectionstring = Getconnectionstring();
        MySqlConnection conn = new MySqlConnection(connectionstring);
        conn.Open();
        string query = "SELECT url FROM audiofile WHERE uploaderid = @Uploaderid";
        MySqlCommand command = new MySqlCommand(query, conn);
        command.Parameters.AddWithValue("@Uploaderid", id);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            string url = reader.GetString("url");
            urls.Add(url);
        }
        conn.Close();
        return urls;
    }
}