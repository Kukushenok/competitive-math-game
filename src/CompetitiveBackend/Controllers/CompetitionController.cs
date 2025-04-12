using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompetitiveBackend.Controllers
{
    [Route($"{APIConsts.ROOTV1}/")]
    [ApiController]
    public class CompetitionController : ControllerBase
    {
        private ICompetitionEditUseCase editUseCase;
        private ICompetitionWatchUseCase watchUseCase;
        public CompetitionController(ICompetitionEditUseCase editUseCase, ICompetitionWatchUseCase watchUseCase)
        {
            this.editUseCase = editUseCase;
            this.watchUseCase = watchUseCase;
        }
        [HttpGet("competitions/{id}")]
        public async Task<ActionResult<CompetitionDTO>> GetCompetition(int id)
        {
            return await watchUseCase.GetCompetition(id);
        }

        [HttpGet("competitions")]
        public async Task<ActionResult<CompetitionDTO[]>> GetAllCompetitions(int page, int count)
        {
            return (await watchUseCase.GetAllCompetitions(new DataLimiterDTO(page, count))).ToArray();
        }
        [HttpGet("competitions/active")]
        public async Task<ActionResult<CompetitionDTO[]>> GetActiveCompetitions()
        {
            return (await watchUseCase.GetActiveCompetitions()).ToArray();
        }
        [HttpPut("competitions")]
        public async Task<ActionResult> CreateCompetition(CompetitionDTO dto)
        {
            using var self = await editUseCase.Auth(HttpContext);
            await self.CreateCompetition(dto);
            return NoContent();
        }
        [HttpPatch("competitions/{id}")]
        public async Task<ActionResult> UpdateCompetition(int id, CompetitionUpdateRequestDTO dto)
        {
            using var self = await editUseCase.Auth(HttpContext);
            await self.UpdateCompetition(new CompetitionUpdateRequestDTO(id, dto));
            return NoContent();
        }
        [HttpPatch("competitions/{id}/level")]
        public async Task<ActionResult> SetCompetitionLevel(int id, IFormFile file)
        {
            using var self = await editUseCase.Auth(HttpContext);
            await self.SetCompetitionLevel(id, await file.ToLargeData());
            return NoContent();
        }
        [HttpGet("competitions/{id}/level")]
        public async Task<FileContentResult> GetCompetitionLevel(int id)
        {
            return (await watchUseCase.GetCompetitionLevel(id)).ToFileResult($"level_{id}.bytes");
        }
    }
}
