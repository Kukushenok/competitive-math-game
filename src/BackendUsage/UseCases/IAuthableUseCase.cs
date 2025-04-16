using System;
using System.Threading.Tasks;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    public interface IAuthableUseCase<T>: IDisposable where T: IAuthableUseCase<T>
    {
        public Task<T> Auth(string token);
    }
}
