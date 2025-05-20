using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompetitiveBackend.Controllers
{
    [Route($"{APIConsts.ROOTV1}/{APIConsts.COMPETITIONS}/")]
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
        [HttpGet("{id}")]
        public async Task<ActionResult<CompetitionDTO>> GetCompetition(int id)
        {
            return await watchUseCase.GetCompetition(id);
        }

        [HttpGet("")]
        public async Task<ActionResult<CompetitionDTO[]>> GetAllCompetitions(int page, int count, string? filter = null)
        {
            CompetitionDTO[] dtos;
            if (filter == "active") dtos = (await watchUseCase.GetActiveCompetitions()).ToArray();
            else dtos = (await watchUseCase.GetAllCompetitions(new DataLimiterDTO(page, count))).ToArray();
            return dtos;
        }
        [HttpPut("")]
        public async Task<ActionResult> CreateCompetition(CompetitionDTO dto)
        {
            using var self = await editUseCase.Auth(HttpContext);
            await self.CreateCompetition(dto);
            return NoContent();
        }
        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateCompetition(int id, CompetitionUpdateRequestDTO dto)
        {
            using var self = await editUseCase.Auth(HttpContext);
            dto.ID = id;
            await self.UpdateCompetition(dto);
            return NoContent();
        }
        //[HttpPatch("{id}/level")]
        //public async Task<ActionResult> SetCompetitionLevel(int id, IFormFile file)
        //{
        //    using var self = await editUseCase.Auth(HttpContext);
        //    await self.SetCompetitionLevel(id, await file.ToLargeData());
        //    return NoContent();
        //}
        [HttpGet($"{{id}}/{APIConsts.LEVEL_FILE}")]
        public async Task<FileContentResult> GetCompetitionLevel(int id,string? platform = null, int? maxVersion = null)
        {
            return (await watchUseCase.GetCompetitionLevel(id, platform, maxVersion)).ToFileResult($"level_{id}.bytes");
        }
    }
}
