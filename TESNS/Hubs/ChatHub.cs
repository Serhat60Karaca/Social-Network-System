using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

using TESNS.Models;
using TESNS.Models.Authentication;

namespace TESNS.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;
        public ChatHub(UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        private static Dictionary<string, string> userConnections = new Dictionary<string, string>();

        public void SendMessageToUser(string userId, string message, string sendid)
        {
            Message mssg = new Message()
            {
                SenderId = Int16.Parse(sendid),
                ReceiverId = Int16.Parse(userId),
                Text = message,
                SendAt = DateTime.Now.ToUniversalTime(),
            };
            _context.Messages.Add(mssg);
            _context.SaveChanges();
            
            if (userConnections.ContainsKey(userId))
            {
                string connectionId = userConnections[userId];
                
                var sender = _context.Users.FirstOrDefault(u => u.Id == Int16.Parse(sendid));
                string username = sender.UserName;
                Clients.Client(connectionId).SendAsync("ReceiveMessage", mssg.Text, username);
            }
        }
        public override Task OnConnectedAsync()
        {
            string userId = Context.UserIdentifier;
            string connectionId = Context.ConnectionId;

            userConnections[userId] = connectionId;

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            string userId = Context.UserIdentifier;

            if (userConnections.ContainsKey(userId))
            {
                userConnections.Remove(userId);
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}
