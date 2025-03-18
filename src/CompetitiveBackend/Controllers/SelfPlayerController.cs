using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CompetitiveBackend.Controllers
{
    public record PlayerProfileDto(string Name, string? Description, string ProfileImageLink);
    [Route("api/self/[action]")]
    public class SelfPlayerController : ControllerBase
    {
        private IPlayerProfileRepository _profileRepo;
        public SelfPlayerController(IPlayerProfileRepository repository)
        {
            _profileRepo = repository;
        }
        [HttpGet("profile")]
        public async Task<IActionResult> GetPlayerProfile()
        {
            SessionToken tok = User.GetSessionToken();
            if (tok.TryGetAccountIdentifier(out int identifier) && tok.Role.IsPlayer())
            {
                PlayerProfile p = await _profileRepo.GetPlayerProfile(identifier);
                return new ObjectResult(new PlayerProfileDto(p.Name, p.Description, "idk sorry :3"));
            }
            return Forbid();
        }
        [HttpPost("profile")]
        public async Task<IActionResult> SetPlayerProfile(string name, string? description)
        {
            SessionToken tok = User.GetSessionToken();
            if (tok.TryGetAccountIdentifier(out int identifier) && tok.Role.IsPlayer())
            {
                try
                {
                    await _profileRepo.UpdatePlayerProfile(new PlayerProfile(name, description, identifier));
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
                return Ok();
            }
            return Forbid();
        }
        [HttpGet("pic")]
        public async Task<IActionResult> GetPlayerImage()
        {
            SessionToken tok = User.GetSessionToken();
            if (tok.TryGetAccountIdentifier(out int identifier) && tok.Role.IsPlayer())
            {
                try
                {
                    LargeData data = await _profileRepo.GetPlayerProfileImage(identifier);
                    return File(data.Data, "application/octet-stream", "Image");
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
            return Forbid();
        }
        [HttpPost("pic")]
        public async Task<IActionResult> SetPlayerImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            SessionToken tok = User.GetSessionToken();
            if (tok.TryGetAccountIdentifier(out int identifier) && tok.Role.IsPlayer())
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                LargeData data = new LargeData(memoryStream.ToArray());
                try
                {
                    await _profileRepo.UpdatePlayerProfileImage(identifier, data);
                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
            return Forbid();
        }
    }
}
