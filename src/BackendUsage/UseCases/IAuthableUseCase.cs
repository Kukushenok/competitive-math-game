using System;
using System.Threading.Tasks;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    public interface IAuthableUseCase<T> : IDisposable
        where T : IAuthableUseCase<T>
    {
        Task<T> Auth(string token);
    }
}
