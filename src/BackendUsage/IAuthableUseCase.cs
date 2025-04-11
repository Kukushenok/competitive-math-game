namespace CompetitiveBackend.BackendUsage
{
    public interface IAuthableUseCase<T>: IDisposable where T: IAuthableUseCase<T>
    {
        public Task<T> Auth(string token);
    }
}
