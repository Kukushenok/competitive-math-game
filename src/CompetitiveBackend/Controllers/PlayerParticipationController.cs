using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.Core.Objects;
using Microsoft.AspNetCore.Mvc;

namespace CompetitiveBackend.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route($"{APIConsts.ROOTV1}/")]
    public class PlayerParticipationController : ControllerBase
    {
        private IPlayerParticipationWatchUseCase _watchUseCase;
        private IPlayerParticipationUseCase _editUseCase;
        public PlayerParticipationController(IPlayerParticipationWatchUseCase watchUseCase, IPlayerParticipationUseCase useCase)
        {
            _editUseCase = useCase;
            _watchUseCase = watchUseCase;
        }
        [HttpGet($"{APIConsts.COMPETITIONS}/{{compID}}/{APIConsts.COMP_PARTICIPATIONS}")]
        public async Task<ActionResult<PlayerParticipationDTO[]>> GetLeaderboard(int compID, int page = 0, int count = 0)
        {
            return (await _watchUseCase.GetLeaderboard(compID, new DataLimiterDTO(page, count))).ToArray();
        }
        [HttpGet($"{APIConsts.COMPETITIONS}/{{compID}}/{APIConsts.COMP_PARTICIPATIONS}/{{profileID}}")]
        public async Task<ActionResult<PlayerParticipationDTO>> GetParticipationInfo(int profileID, int compID)
        {
            return (await _watchUseCase.GetParticipation(compID, profileID));
        }
        [HttpDelete($"{APIConsts.COMPETITIONS}/{{compID}}/{APIConsts.COMP_PARTICIPATIONS}/{{profileID}}")]
        public async Task<NoContentResult> DeleteParticipation(int profileID, int compID)
        {
            using var self = await _editUseCase.Auth(HttpContext);
            await self.DeleteParticipation(compID, profileID);
            return NoContent(); 
        }
        [HttpPut($"{APIConsts.COMPETITIONS}/{{compID}}/{APIConsts.COMP_PARTICIPATIONS}")]
        public async Task<NoContentResult> ApplyInCompetition(int compID, int score)
        {
            using var self = await _editUseCase.Auth(HttpContext);
            await self.SubmitScoreTo(compID, score);
            return NoContent();
        }
        [HttpGet($"{APIConsts.PLAYERS}/{APIConsts.SELF}/{APIConsts.PLAYER_PARTICIPATIONS}")]
        public async Task<ActionResult<PlayerParticipationDTO[]>> GetMyParticipations(int page = 0, int count = 0)
        {
            using var self = await _editUseCase.Auth(HttpContext);
            return (await self.GetMyParticipations(new DataLimiterDTO(page, count))).ToArray();
        }
    }
}
