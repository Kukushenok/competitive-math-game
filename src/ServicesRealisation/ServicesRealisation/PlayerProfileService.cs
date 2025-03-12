using CompetitiveBackend.Core;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.Objects;
using Microsoft.Extensions.Logging;

namespace CompetitiveBackend.Services.PlayerProfileService
{
    public class PlayerProfileService : IPlayerProfileService
    {
        private readonly ILogger<PlayerProfileService> _logger;
        private readonly IPlayerProfileRepository _profileRepository;
        private readonly ILargeFileProcessor _imageProcessor;
        public PlayerProfileService(IPlayerProfileRepository profileRepository, ILogger<PlayerProfileService> logger, ILargeFileProcessor processor)
        {
            _profileRepository = profileRepository;
            _logger = logger;
            _imageProcessor = processor;
        }

        public Task<PlayerProfile> GetPlayerProfile(int playerProfileId)
        {
            return _profileRepository.GetPlayerProfile(playerProfileId);
        }

        public Task<LargeData> GetPlayerProfileImage(int playerProfileId)
        {
            return _profileRepository.GetPlayerProfileImage(playerProfileId);
        }

        public async Task SetPlayerProfileImage(int playerID, LargeData data)
        {
            await _profileRepository.UpdatePlayerProfileImage(playerID, data);
        }

        public Task UpdatePlayerProfile(PlayerProfile p)
        {
            return _profileRepository.UpdatePlayerProfile(p);
        }

        public async Task UpdatePlayerProfileImage(int playerProfileId, LargeData data)
        {
            LargeData processed = await _imageProcessor.Process(data);
            await _profileRepository.UpdatePlayerProfileImage(playerProfileId, processed);
        }
    }
}
