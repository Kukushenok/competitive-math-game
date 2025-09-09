using CompetitiveBackend.BackendUsage.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    public interface IPlayerProfileUseCase
    {
        public Task<PlayerProfileDTO> GetProfile(int id);
        public Task<LargeDataDTO> GetProfileImage(int id);
    }
}
