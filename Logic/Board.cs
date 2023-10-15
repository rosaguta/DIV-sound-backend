using TagLib.Mpeg;

namespace Logic;

public class Board
{
    public int Id { get; set; }
    public string name { get; set; }
    public List<Audiofile> AudioList { get; set; }
    
    public Board()
    {
        AudioList = new List<Audiofile>();
    }
}