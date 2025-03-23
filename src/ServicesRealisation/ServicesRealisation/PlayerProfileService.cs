using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.ExtraTools;
using ServicesRealisation.ServicesRealisation.Validator;
using System.ComponentModel.DataAnnotations;

namespace CompetitiveBackend.Services.PlayerProfileService
{
    public class PlayerProfileService : IPlayerProfileService
    {
        private readonly IPlayerProfileRepository _profileRepository;
        private readonly IImageProcessor _imageProcessor;
        private readonly IValidator<PlayerProfile> _validator;
        public PlayerProfileService(IPlayerProfileRepository profileRepository, IImageProcessor processor, IValidator<PlayerProfile> validator)
        {
            _profileRepository = profileRepository;
            _imageProcessor = processor;
            _validator = validator;
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
            LargeData processed = await _imageProcessor.Process(data);
            await _profileRepository.UpdatePlayerProfileImage(playerID, processed);
        }

        public Task UpdatePlayerProfile(PlayerProfile p)
        {
            if (!_validator.IsValid(p, out string? msg)) throw new Exceptions.InvalidArgumentsException(msg!);
            return _profileRepository.UpdatePlayerProfile(p);
        }
    }
}
