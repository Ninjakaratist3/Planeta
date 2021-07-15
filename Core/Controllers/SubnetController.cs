using Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers
{
    public class SubnetController : Controller
    {
        private readonly ISubnetRepository _subnetRepository;

        public SubnetController(ISubnetRepository subnetRepository)
        {
            _subnetRepository = subnetRepository;
        }
    }
}
