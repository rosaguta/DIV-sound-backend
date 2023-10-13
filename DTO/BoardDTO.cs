namespace DTO;

public class BoardDTO
{
    public int Id { get; private set; }
    public string name { get; private set; }
    public List<AudiofileDTO> AudioList { get; private set; }
}