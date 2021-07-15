using Core.Services.User;
using Core.ViewModels.User;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers
{
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _userService.Get(id);

            return Ok(user);
        }

        [HttpPost]
        public IActionResult Create(UserForm model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            _userService.Create(model);

            return Ok();
        }

        [HttpPut]
        public IActionResult Update(UserForm model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            _userService.Update(model);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);

            return Ok();
        }
    }
}
