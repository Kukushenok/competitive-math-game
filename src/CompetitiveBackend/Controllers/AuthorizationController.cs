using CompetitiveBackend.Repositories.Exceptions;
using CompetitiveBackend.Services;
using CompetitiveBackend.Services.Exceptions;
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
        public async Task<IActionResult> Login(string login, string password)
        {
            try
            {
                AuthSuccessResult result = await Service.LogIn(login, password);
                return Ok(result);
            }
            catch(MissingDataException)
            {
                return NotFound();
            }
            catch(IncorrectPasswordException)
            {
                return Forbid();
            }
            //try
            //{
            //}
            //catch
            //{
            //    return BadRequest(":(");
            //}
        }
        [HttpPost(Name = "register")]
        public async Task<IActionResult> Register(string login, string password)
        {
            //try
            //{
            try
            {
                await Service.Register(new Core.Objects.Account(login, null, null, null), password);
                return Ok();
            }
            catch (RepositoryException exp)
            {
                return BadRequest(exp.Message);
            }

            //}
            //catch
            //{
            //    return BadRequest();
            //}
        }
    }
}
