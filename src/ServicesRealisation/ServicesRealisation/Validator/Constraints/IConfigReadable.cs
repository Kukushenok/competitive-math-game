using Microsoft.Extensions.Configuration;

namespace ServicesRealisation.ServicesRealisation.Validator.Constraints
{
    public interface IConfigReadable
    {
        void Read(IConfigurationSection section);
    }
}
