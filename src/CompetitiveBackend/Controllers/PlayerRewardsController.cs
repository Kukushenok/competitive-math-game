using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.Core.Objects;
using Microsoft.AspNetCore.Mvc;

namespace CompetitiveBackend.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route($"{APIConsts.ROOTV1}/{APIConsts.PLAYER}/")]
    public class PlayerRewardsController: ControllerBase
    {
        private IPlayerRewardUseCase _useCase;
        public PlayerRewardsController(IPlayerRewardUseCase useCase)
        {
            _useCase = useCase;
        }
        [HttpGet($"{APIConsts.SELF}/{APIConsts.REWARD}")]
        public async Task<ActionResult<PlayerRewardDTO[]>> GetMyRewards(int page, int count)
        {
            using var self = await _useCase.Auth(HttpContext);
            return (await self.GetAllMineRewards(new DataLimiterDTO(page, count))).ToArray();
        }
        [HttpGet($"{{playerID}}/{APIConsts.REWARD}")]
        public async Task<ActionResult<PlayerRewardDTO[]>> GetMyRewards(int playerID, int page, int count)
        {
            using var self = await _useCase.Auth(HttpContext);
            return (await self.GetAllRewardsOf(playerID, new DataLimiterDTO(page, count))).ToArray();
        }
        [HttpPut($"{{playerID}}/{APIConsts.REWARD}")]
        public async Task<ActionResult> GrantRewardTo(int playerID, int rewardDescriptionID)
        {
            using var self = await _useCase.Auth(HttpContext);
            await self.GrantRewardToPlayer(playerID, rewardDescriptionID);
            return NoContent();
        }
        [HttpDelete($"{APIConsts.REWARD}/{{rewardID}}")]
        public async Task<ActionResult> RemoveReward(int rewardID)
        {
            using var self = await _useCase.Auth(HttpContext);
            await self.DeleteReward(rewardID);
            return NoContent();
        }
    }
}
