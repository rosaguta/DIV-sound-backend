using Microsoft.AspNetCore.Mvc;
using Logic;

namespace DIV_SOUND_backend.Controllers;
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    [HttpPost]
    [Route("/Users/Register")]
    public IActionResult User([FromBody] User user)
    {
        UserCollection userCollection = new UserCollection();
        bool alreadyexists = userCollection.Register(user.Firstname, user.Lastname, user.Passhash, user.Mail, user.Username);
        if (alreadyexists == true)
        {
            return Ok();
        }

        return BadRequest("User already exists :3");
    }

    [HttpGet]
    [Route("/Users")]
    public List<User> Users()
    {
        UserCollection usercollection = new UserCollection();
        List<User> userlist = usercollection.GetUsers();
        return userlist;
    }

    [HttpGet]
    [Route("/Users/{username}")]
    public User User(string username)
    {
        UserCollection userCollection = new UserCollection();
        User user = userCollection.GetUser(username);
        return user;
    }

}