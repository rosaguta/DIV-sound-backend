using DALInterface;
using DTO;
using Logic.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace Logic;

public class Audiofile
{
    public int? Id { get; set; }
    public string? Filename { get; set; }
    public float? Duration { get; set; }
    public string? Path { get; set; }
    public TimeSpan? Uploaddate { get; set; }
    public int Uploaderid { get; set; }
    public string url { get; set; }


}