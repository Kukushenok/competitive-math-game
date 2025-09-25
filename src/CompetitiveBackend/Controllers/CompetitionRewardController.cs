using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CompetitiveBackend.Controllers
{
    [Route($"{APIConsts.ROOTV1}/{APIConsts.COMPETITIONS}/")]
    [ApiController]
    public class CompetitionRewardController: ControllerBase
    {
        private ICompetitionRewardEditUseCase _editRewards;
        private ICompetitionWatchUseCase _watchRewards;
        public CompetitionRewardController(ICompetitionRewardEditUseCase editRewards, ICompetitionWatchUseCase service)
        {
            _editRewards = editRewards;
            _watchRewards = service;
        }
        [HttpGet($"{{compID}}/{APIConsts.REWARDS}")]
        public async Task<IEnumerable<CompetitionRewardDTO>> GetCompetitionRewards(int compID)
        {
            return await _watchRewards.GetRewardsFor(compID);
        }
        [HttpPost($"{{compID}}/{APIConsts.REWARDS}")]
        public async Task<ActionResult> CreateCompetitionReward(int compID, UpdateCompetitionRewardDTO rewardDTO)
        {
            CreateCompetitionRewardDTO creating = new CreateCompetitionRewardDTO(null, rewardDTO.RewardDescriptionID, compID, rewardDTO.ConditionByRank, rewardDTO.ConditionByPlace);
            using var self = await _editRewards.Auth(HttpContext);
            await self.CreateCompetitionReward(creating);
            return NoContent();
        }
        [HttpDelete($"{{compID}}/{APIConsts.REWARDS}/{{rewardID}}")]
        public async Task<ActionResult> DeleteCompetitionReward(int compID, int rewardID)
        {
            using var self = await _editRewards.Auth(HttpContext);
            await self.RemoveCompetitionReward(rewardID);
            return NoContent();
        }
        [HttpPatch($"{{compID}}/{APIConsts.REWARDS}/{{rewardID}}")]
        public async Task<ActionResult> UpdateCompetitionReward(int compID, int rewardID, UpdateCompetitionRewardDTO rewardDTO)
        {
            UpdateCompetitionRewardDTO dto = new UpdateCompetitionRewardDTO(rewardID, rewardDTO.RewardDescriptionID, rewardDTO.ConditionByRank, rewardDTO.ConditionByPlace);
            using var self = await _editRewards.Auth(HttpContext);
            await self.UpdateCompetitionReward(dto);
            return NoContent();
        }
    }
}
