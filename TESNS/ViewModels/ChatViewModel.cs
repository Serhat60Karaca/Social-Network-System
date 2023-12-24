namespace TESNS.ViewModels;

public class ChatViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Image { get; set; }
    public string? LastMessage { get; set; }
    public DateTime? LastMessageDate { get; set; }
}