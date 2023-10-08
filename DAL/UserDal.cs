using System.Data;
using DALInterface;
using DTO;
//using System.Data.SqlClient;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
//using MySqlConnector;
//    "SqlServer": "Data Source=138.201.52.251,32824;database=DIVSOUND;User ID=root;Password=root;",
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
        string query = "SELECT * FROM [user] WHERE [username] = @username";
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
            return userdto;
        }
        return null;
    }

    public bool NewUser(UserDTO userDto)
    {
        string? connectionstring = Getconnectionstring();
        MySqlConnection conn = new MySqlConnection(connectionstring);
        conn.Open();
        string query = "INSERT INTO [user] (firstname, lastname, username, mail, passhash)" +
                       "VALUES(@Firstname, @Lastname, @Username, @Mail, @Passhash)";
        using (MySqlCommand cmd = new MySqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@Firstname", userDto.Firstname);
            cmd.Parameters.AddWithValue("@Lastname", userDto.Lastname);
            cmd.Parameters.AddWithValue("@Username", userDto.Username);
            cmd.Parameters.AddWithValue("@Mail", userDto.Mail);
            cmd.Parameters.AddWithValue("@Passhash",userDto.Passhash);
            int rowsaffected = cmd.ExecuteNonQuery();
            return rowsaffected > 0;
        }


    }
}