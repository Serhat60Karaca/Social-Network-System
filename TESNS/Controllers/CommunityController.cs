using Microsoft.AspNetCore.Mvc;
using TESNS.Repositories;
using TESNS.Repositories.Concrete;

namespace TESNS.Controllers
{
    public class CommunityController : Controller
    {
        private readonly ICommunityRepository _communityRepository;
        public CommunityController(ICommunityRepository communityRepository) 
        {
            _communityRepository = communityRepository;
        }
        public IActionResult Index()
        {
            var comm =_communityRepository.GetAllCommunities();
            return View(comm);
        }
    }
}
