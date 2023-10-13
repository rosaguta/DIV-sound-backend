using TagLib.Mpeg;

namespace Logic;

public class Board
{
    public int Id { get; private set; }
    public string name { get; private set; }
    public List<Audiofile> AudioList { get; private set; }
    
    
}