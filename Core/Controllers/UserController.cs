using Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
    }
}
