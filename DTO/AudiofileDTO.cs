using Microsoft.AspNetCore.Http;

namespace DTO
{
    public class AudiofileDTO
    {
        public int? Id { get; set; }
        public string? Filename { get; set; }
        public double? Duration { get; set; }
        public string? Path { get; set; }
        public DateTime? Uploaddate { get; set; }
        public int? Uploaderid { get; set; }
        public string? url { get; set; }

    }
}