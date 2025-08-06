namespace ShuppiApi.Dtos;

public class TagDto
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public bool IsHidden { get; set; }
  public List<int> CategoryIds { get; set; } = new();
}
