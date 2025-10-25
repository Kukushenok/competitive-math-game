using Microsoft.Extensions.Configuration;
using ServicesRealisation.ServicesRealisation.Validator.Constraints;

namespace ServicesRealisation.ServicesRealisation.Validator
{
    public abstract class ConfigurationValidator<T> : IValidator<T>
    {
        public const string CONSTRAINTS = "Constraints";
        private readonly string sTARTSTR;
        private readonly IConfiguration configuration;
        public ConfigurationValidator(IConfiguration configuration, string? typeName = null)
        {
            this.configuration = configuration;
            typeName ??= typeof(T).Name;
            sTARTSTR = $"{CONSTRAINTS}:{typeName}:";
        }

        public abstract bool IsValid(T value, out string? msg);
        protected TOther Read<TOther>(in TOther data, string keyIdx)
            where TOther : IConfigReadable
        {
            data.Read(configuration.GetSection(sTARTSTR + keyIdx));
            return data;
        }
    }
}
