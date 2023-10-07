using Microsoft.AspNetCore.Http;

namespace DTO
{
    public class AudiofileDTO
    {
        public string filename { get; set; }
        public float duration { get; set; }
        public string filetype { get; set; }
        public string path { get; set; }
        public IFormFile file { get; set; }

        public AudiofileDTO()
        {
            
        }
    }
}