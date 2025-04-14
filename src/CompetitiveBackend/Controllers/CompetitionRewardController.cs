using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CompetitiveBackend.Controllers
{
    [Route($"{APIConsts.ROOTV1}/{APIConsts.COMPETITION}/")]
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
        [HttpGet($"{{compID}}/{APIConsts.REWARD}")]
        public async Task<IEnumerable<CompetitionRewardDTO>> GetCompetitionRewards(int compID)
        {
            return await _watchRewards.GetRewardsFor(compID);
        }
        [HttpPut($"{{compID}}/{APIConsts.REWARD}")]
        public async Task<ActionResult> CreateCompetitionReward(int compID, UpdateCompetitionRewardDTO rewardDTO)
        {
            CreateCompetitionRewardDTO creating = new CreateCompetitionRewardDTO(null, rewardDTO.RewardDescriptionID, compID) { ConditionByPlace = rewardDTO.ConditionByPlace, ConditionByRank = rewardDTO.ConditionByRank };
            using var self = await _editRewards.Auth(HttpContext);
            await self.CreateCompetitionReward(creating);
            return NoContent();
        }
        [HttpDelete($"{APIConsts.REWARD}/{{rewardID}}")]
        public async Task<ActionResult> DeleteCompetitionReward(int rewardID)
        {
            using var self = await _editRewards.Auth(HttpContext);
            await self.RemoveCompetitionReward(rewardID);
            return NoContent();
        }
        [HttpPatch($"{APIConsts.REWARD}/{{rewardID}}")]
        public async Task<ActionResult> UpdateCompetitionReward(int rewardID, UpdateCompetitionRewardDTO rewardDTO)
        {
            UpdateCompetitionRewardDTO dto = new UpdateCompetitionRewardDTO(rewardID, rewardDTO.RewardDescriptionID) { ConditionByPlace = rewardDTO.ConditionByPlace, ConditionByRank = rewardDTO.ConditionByRank };
            using var self = await _editRewards.Auth(HttpContext);
            await self.UpdateCompetitionReward(dto);
            return NoContent();
        }
    }
}
