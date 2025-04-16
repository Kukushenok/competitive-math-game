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
        [HttpGet($"{APIConsts.COMPETITIONS}/{{competitionID}}/{APIConsts.COMP_PARTICIPATIONS}")]
        public async Task<ActionResult<PlayerParticipationDTO[]>> GetLeaderboard(int competitionID, int page = 0, int count = 0)
        {
            return (await _watchUseCase.GetLeaderboard(competitionID, new DataLimiterDTO(page, count))).ToArray();
        }
        [HttpGet($"{APIConsts.COMPETITIONS}/{{competitionID}}/{APIConsts.COMP_PARTICIPATIONS}/{{profileID}}")]
        public async Task<ActionResult<PlayerParticipationDTO>> GetParticipationInfo(int profileID, int competitionID)
        {
            return (await _watchUseCase.GetParticipation(competitionID, profileID));
        }
        [HttpDelete($"{APIConsts.COMPETITIONS}/{{competitionID}}/{APIConsts.COMP_PARTICIPATIONS}/{{profileID}}")]
        public async Task<ActionResult> DeleteParticipation(int profileID, int competitionID)
        {
            using var self = await _editUseCase.Auth(HttpContext);
            await self.DeleteParticipation(competitionID, profileID);
            return NoContent(); 
        }
        [HttpPut($"{APIConsts.COMPETITIONS}/{{competitionID}}/{APIConsts.COMP_PARTICIPATIONS}")]
        public async Task<ActionResult> ApplyInCompetition(int competitionID, int score)
        {
            using var self = await _editUseCase.Auth(HttpContext);
            await self.SubmitScoreTo(competitionID, score);
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
