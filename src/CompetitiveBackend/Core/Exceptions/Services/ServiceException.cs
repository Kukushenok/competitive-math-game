namespace CompetitiveBackend.Core.Exceptions.Services
{
    public abstract class ServiceException: Exception
    {
        public override string Message => SourceClass() + " " + base.Message;
        protected abstract string SourceClass();
    }
    public class ServiceException<T>: ServiceException where T : class
    {
        protected sealed override string SourceClass() => typeof(T).ToString();
    }
}
