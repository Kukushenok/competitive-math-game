using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.ExtraTools;
using ServicesRealisation.ServicesRealisation.Validator;

namespace CompetitiveBackend.Services.PlayerProfileService
{
    public class PlayerProfileService : IPlayerProfileService
    {
        private readonly IPlayerProfileRepository profileRepository;
        private readonly IImageProcessor imageProcessor;
        private readonly IValidator<PlayerProfile> validator;
        public PlayerProfileService(IPlayerProfileRepository profileRepository, IImageProcessor processor, IValidator<PlayerProfile> validator)
        {
            this.profileRepository = profileRepository;
            imageProcessor = processor;
            this.validator = validator;
        }

        public Task<PlayerProfile> GetPlayerProfile(int playerID)
        {
            return profileRepository.GetPlayerProfile(playerID);
        }

        public Task<LargeData> GetPlayerProfileImage(int playerID)
        {
            return profileRepository.GetPlayerProfileImage(playerID);
        }

        public async Task SetPlayerProfileImage(int playerID, LargeData data)
        {
            LargeData processed = await imageProcessor.Process(data);
            await profileRepository.UpdatePlayerProfileImage(playerID, processed);
        }

        public Task UpdatePlayerProfile(PlayerProfile p)
        {
            return !validator.IsValid(p, out string? msg)
                ? throw new Exceptions.InvalidArgumentsException(msg!)
                : profileRepository.UpdatePlayerProfile(p);
        }
    }
}
