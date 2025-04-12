using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompetitiveBackend.Controllers
{
    [Route($"{APIConsts.ROOTV1}/")]
    [ApiController]
    public class RewardDescriptionController : ControllerBase
    {
        private IRewardDescriptionEditUseCase editUseCase;
        private IRewardDescriptionWatchUseCase watchUseCase;
        public RewardDescriptionController(IRewardDescriptionEditUseCase editUseCase, IRewardDescriptionWatchUseCase watchUseCase)
        {
            this.editUseCase = editUseCase;
            this.watchUseCase = watchUseCase;
        }
        [HttpGet("reward_descriptions/{id}")]
        public async Task<ActionResult<RewardDescriptionDTO>> GetRewardDescription(int id)
        {
            return await watchUseCase.GetRewardDescription(id);
        }

        [HttpGet("reward_descriptions")]
        public async Task<ActionResult<RewardDescriptionDTO[]>> GetAllRewardDescriptions(int page, int count)
        {
            return (await watchUseCase.GetAllRewardDescriptions(new DataLimiterDTO(page, count))).ToArray();
        }
        [HttpGet("reward_descriptions/{id}/image")]
        public async Task<FileContentResult> GetRewardIcon(int id)
        {
            return (await watchUseCase.GetRewardIcon(id)).ToFileResult($"reward_{id}.jpg");
        }
        [HttpGet("reward_descriptions/{id}/asset")]
        public async Task<FileContentResult> GetRewardGameAsset(int id)
        {
            return (await watchUseCase.GetRewardGameAsset(id)).ToFileResult($"reward_{id}_asset.bytes");
        }
        [HttpPut("reward_descriptions")]
        public async Task<ActionResult> CreateReward(RewardDescriptionDTO dto)
        {
            using var self = await editUseCase.Auth(HttpContext);
            await self.CreateRewardDescription(dto);
            return NoContent();
        }
        [HttpPatch("reward_descriptions/{id}")]
        public async Task<ActionResult> UpdateRewardDescription(RewardDescriptionDTO dto)
        {
            using var self = await editUseCase.Auth(HttpContext);
            await self.UpdateRewardDescription(dto);
            return NoContent();
        }
        [HttpPatch("reward_descriptions/{id}/image")]
        public async Task<ActionResult> UpdateRewardDescriptionIcon(int id, IFormFile file)
        {
            using var self = await editUseCase.Auth(HttpContext);
            await self.SetRewardIcon(id, await file.ToLargeData());
            return NoContent();
        }
        [HttpPatch("reward_descriptions/{id}/asset")]
        public async Task<ActionResult> UpdateRewardGameAsset(int id, IFormFile file)
        {
            using var self = await editUseCase.Auth(HttpContext);
            await self.SetRewardGameAsset(id, await file.ToLargeData());
            return NoContent();
        }
    }
}
