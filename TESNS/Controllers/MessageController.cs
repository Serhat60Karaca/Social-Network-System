using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TESNS.Models;
using TESNS.Models.Authentication;
using TESNS.Repositories.Concrete;
using TESNS.ViewModels;

namespace TESNS.Controllers;

public class MessageController : Controller
{
    
    private readonly ApplicationDbContext _context;
    private readonly UserRepository _userRepository;
    private readonly UserManager<AppUser> _userManager;
    
    public MessageController(ApplicationDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userRepository = new UserRepository(_context);
        _userManager = userManager;
    }
    
    public IActionResult Index()
    {
        return View();
    }
    
    public async Task<IActionResult> Chat()
    {
        var userId = int.Parse(_userManager.GetUserId(User));
        var chatViewModels = await GetChatViewModels(userId);
        ViewBag.ChatViewModels = chatViewModels;
        
        ViewData["ChatUsers"] = chatViewModels;
        ViewData["ChatUser"] = null;
        
        return View();
    }
    
    [Route("{id:int}")]
    public async Task<IActionResult> Chat(int id)
    {
        var userId = int.Parse(_userManager.GetUserId(User));
        var chatViewModels = await GetChatViewModels(userId);
        ViewBag.ChatViewModels = chatViewModels;
        
        ViewData["ChatUsers"] = chatViewModels;
        ViewData["ChatUser"] = await GetChatUserViewModel(userId, id);
        
        
        return View();
    }
    
    public async Task<ChatUserViewModel> GetChatUserViewModel(int userId, int chatUserId)
    {
        if (chatUserId == 0)
        {
            return null;
        }
        
        var chatUser = await _userRepository.GetUserById(chatUserId);
        var chatUserViewModel = new ChatUserViewModel
        {
            Id = chatUser.Id,
            Name = chatUser.UserName,
            Messages = await _context.Messages
                .Where(message => message.ReceiverId == userId && message.SenderId == chatUserId ||
                                  message.ReceiverId == chatUserId && message.SenderId == userId)
                .OrderBy(message => message.SendAt)
                .ToListAsync()
        };
        
        return chatUserViewModel;
    }

    public async Task<List<ChatViewModel>> GetChatViewModels(int userId)
    {
        var users = await _userRepository.GetAllUsers();
        
        var chatViewModels = new List<ChatViewModel>();
        
        foreach (var user in users)
        {
            var lastMessage = _context.Messages
                .OrderBy(m => m.SendAt)
                .LastOrDefault(message => message.ReceiverId == userId && message.SenderId == user.Id ||
                                          message.ReceiverId == user.Id && message.SenderId == userId);

            if (userId == user.Id)
            {
                continue;
            }
            
            if (lastMessage == null)
            {
                chatViewModels.Add(new ChatViewModel
                {
                    Id = user.Id,
                    Name = user.UserName,
                    Image = user.ProfilePhoto
                });
                continue;
            }
            
            var chatViewModel = new ChatViewModel
            {
                Id = user.Id,
                Name = user.UserName,
                Image = user.ProfilePhoto,
                LastMessage = lastMessage.Text,
                LastMessageDate = lastMessage.SendAt
            };
            
            chatViewModels.Add(chatViewModel);
        }
        
        return chatViewModels.OrderByDescending(chatViewModels => chatViewModels.LastMessageDate).ToList();
    }
}