using TESNS.Models;

namespace TESNS.ViewModels;

public class ChatUserViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Message> Messages { get; set; }
}