using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
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
    [HttpGet]
    [Route("/Users/Login")]
    public IActionResult Login(string email, string Password)
    {
        User user = new User();
        user = user.Try_GetUser(email);
        if (user != null)
        {
            bool v = user.CheckPassword(Password, user);
            if (v == false)
            {
                return Unauthorized(new LoginResponse { Message = "UnAuthorized", StatusCode = 401, User = null });
            }
            else
            {
                return Ok(new LoginResponse { Message = "Authorized", StatusCode = 200, User = user });
            }
        }
        else
        {
            return Unauthorized(new LoginResponse { Message = "UnAuthorized", StatusCode = 401, User = null });
        }
    }


}