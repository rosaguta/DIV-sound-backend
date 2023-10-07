namespace Logic;

public class User
{
    public int Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Mail { get; set; }
    public string Username { get; set; }
    public string Passhash { get; set; }

    public User()
    {
    }

    public User(string firstname, string lastname, string mail, string username, string passhash)
    {
        Firstname = firstname;
        Lastname = lastname;
        Mail = mail;
        Username = username;
        Passhash = passhash;
    }
}