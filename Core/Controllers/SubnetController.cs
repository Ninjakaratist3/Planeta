using Core.Services.Subnet;
using Core.ViewModels.Subnet;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers
{
    [Route("subnet")]
    public class SubnetController : Controller
    {
        private readonly ISubnetService _subnetService;

        public SubnetController(ISubnetService subnetService)
        {
            _subnetService = subnetService;
        }

        [HttpGet("{userId}")]
        public IActionResult Get(int userId)
        {
            var user = _subnetService.Get(userId);

            return Ok(user);
        }

        [HttpPost]
        public IActionResult Create(SubnetForm model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            _subnetService.Create(model);

            return Ok();
        }

        [HttpPut]
        public IActionResult Update(SubnetForm model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            _subnetService.Update(model);

            return Ok();
        }

        [HttpDelete("{userId}")]
        public IActionResult Delete(int userId)
        {
            _subnetService.Delete(userId);

            return Ok();
        }
    }
}
