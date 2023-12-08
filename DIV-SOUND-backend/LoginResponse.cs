using Logic;
namespace DIV_SOUND_backend
{
    public class LoginResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public User User { get; set; }

    }
}
