namespace DTO;

public class BoardDTO
{
    public int Id { get; set; }
    public string name { get; set; }
    public List<AudiofileDTO> AudioList { get; set; }

    public BoardDTO() 
    {
        AudioList = new List<AudiofileDTO>();
    }
}