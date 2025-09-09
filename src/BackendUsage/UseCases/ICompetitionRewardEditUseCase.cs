using CompetitiveBackend.BackendUsage.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    public interface ICompetitionRewardEditUseCase: IAuthableUseCase<ICompetitionRewardEditUseCase>
    {
        public Task CreateCompetitionReward(CreateCompetitionRewardDTO reward);
        public Task UpdateCompetitionReward(UpdateCompetitionRewardDTO reward);
        public Task RemoveCompetitionReward(int compRewardID);
    }
}
