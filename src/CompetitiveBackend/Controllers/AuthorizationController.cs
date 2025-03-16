using CompetitiveBackend.Services;
using CompetitiveBackend.Services.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CompetitiveBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private IAuthService Service;
        public AuthorizationController(IAuthService service)
        {
            Service = service;
        }
        [HttpGet(Name = "admin")]
        [Authorize(Policy = "Admin")]
        public string AdminSecrets()
        {
            return ":3";
        }

        [HttpGet(Name = "player")]
        [Authorize(Policy = "Player")]
        public string PlayerSecrets()
        {
            return ":3";
        }
        [HttpPost(Name = "login")]
        public async Task<ObjectResult> Login(string login, string password)
        {
            //try
            //{
            return Ok(await Service.LogIn(login, password));
            //}
            //catch
            //{
            //    return BadRequest(":(");
            //}
        }
        [HttpPost(Name = "register")]
        public async Task<StatusCodeResult> Register(string login, string password)
        {
            //try
            //{
            await Service.Register(new Core.Objects.Account(login, null, null, null), password);
            return Ok();
            //}
            //catch
            //{
            //    return BadRequest();
            //}
        }
    }
}
