namespace ServicesRealisation.ServicesRealisation.Validator
{
    public interface IValidator<T>
    {
        bool IsValid(T value, out string? msg);
    }
}
