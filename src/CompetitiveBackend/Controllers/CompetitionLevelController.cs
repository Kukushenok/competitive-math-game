using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CompetitiveBackend.Controllers
{
    [Route($"{APIConsts.ROOTV1}/{APIConsts.COMPETITIONS}/")]
    [ApiController]
    public class CompetitionLevelController: ControllerBase
    {
        [Serializable]
        public class LevelAddRequestDTO
        {
            public string Platform { get; set; }
            public int VersionKey { get; set; }
            public IFormFile File { get; set; }
        }
        private ICompetitionLevelEditUseCase competitionLevelEditUseCase;
        public CompetitionLevelController(ICompetitionLevelEditUseCase competitionLevelEditUseCase)
        {
            this.competitionLevelEditUseCase = competitionLevelEditUseCase;
        }

        [HttpPut($"{{id}}/{APIConsts.LEVELS}")]
        public async Task<ActionResult> AddCompetitionLevel(int id, [FromForm] LevelAddRequestDTO requestDTO)
        {
            using var self = await competitionLevelEditUseCase.Auth(HttpContext);
            LevelDataInfoDTO dto = new LevelDataInfoDTO(id, requestDTO.Platform, requestDTO.VersionKey, null);
            LargeDataDTO fdto = await requestDTO.File.ToLargeData();
            dto.ID = null;
            await self.AddLevelToCompetition(dto, fdto);
            return NoContent();
        }
        [HttpGet($"{{id}}/{APIConsts.LEVELS}")]
        public async Task<ActionResult<LevelDataInfoDTO[]>> GetCompetitionLevels(int id)
        {
            using var self = await competitionLevelEditUseCase.Auth(HttpContext);
            return (await self.GetLevelInfos(id)).ToArray();
        }
        [HttpPatch($"{{unused}}/{APIConsts.LEVELS}/{{id}}/{APIConsts.INFO}")]
        public async Task<ActionResult> ChangeCompetitionLevelData(int id, LevelDataInfoDTO levelDataInfoDTO, int unused = 0)
        {
            levelDataInfoDTO.ID = id;
            using var self = await competitionLevelEditUseCase.Auth(HttpContext);
            await self.UpdateLevelDataInfo(levelDataInfoDTO);
            return NoContent();
        }
        [HttpPatch($"{{unused}}/{APIConsts.LEVELS}/{{id}}/{APIConsts.LEVEL_FILE}")]
        public async Task<ActionResult> ChangeCompetitionLevel(int id, IFormFile file, int unused = 0)
        {
            using var self = await competitionLevelEditUseCase.Auth(HttpContext);
            await self.UpdateLevelData(id, await file.ToLargeData());
            return NoContent();
        }
        [HttpDelete($"{{unused}}/{APIConsts.LEVELS}/{{id}}")]
        public async Task<ActionResult> RemoveCompetitionLevel(int id, int unused = 0)
        {
            using var self = await competitionLevelEditUseCase.Auth(HttpContext);
            await self.DeleteLevel(id);
            return NoContent();
        }
        [HttpGet($"{{unused}}/{APIConsts.LEVELS}/{{id}}/{APIConsts.LEVEL_FILE}")]
        public async Task<FileContentResult> GetSpecificCompetitionLevel(int id, int unused = 0)
        {
            using var self = await competitionLevelEditUseCase.Auth(HttpContext);
            return (await self.GetSpecificCompetitionData(id)).ToFileResult($"level_{id}.bytes");
        }
        [HttpGet($"{{unused}}/{APIConsts.LEVELS}/{{id}}/{APIConsts.INFO}")]
        public async Task<ActionResult<LevelDataInfoDTO>> GetSpecificCompetitionLevelInfo(int id, int unused = 0)
        {
            using var self = await competitionLevelEditUseCase.Auth(HttpContext);
            return await self.GetSpecificCompetitionInfo(id);
        }
    }
}
