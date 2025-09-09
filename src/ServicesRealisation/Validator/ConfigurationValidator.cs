using Microsoft.Extensions.Configuration;
using ServicesRealisation.ServicesRealisation.Validator.Constraints;

namespace ServicesRealisation.ServicesRealisation.Validator
{
    public abstract class ConfigurationValidator<T>: IValidator<T> 
    {
        public const string CONSTRAINTS = "Constraints";
        private string START_STR;
        private IConfiguration _configuration;
        public ConfigurationValidator(IConfiguration configuration, string? typeName = null)
        {
            this._configuration = configuration;
            typeName ??= typeof(T).Name;
            START_STR = $"{CONSTRAINTS}:{typeName}:";
        }

        public abstract bool IsValid(T value, out string? msg);
        protected B Read<B>(in B data, string keyIdx) where B: IConfigReadable
        {
            data.Read(_configuration.GetSection(START_STR + keyIdx));
            return data;
        }
    }
}
