using Microsoft.Extensions.Configuration;

namespace ServicesRealisation.ServicesRealisation.Validator.Constraints
{
    public class MinMaxIntConstraint : IValidator<int>, IConfigReadable
    {
        public int Min { get; private set; }
        public int Max { get; private set; }
        public MinMaxIntConstraint(int min, int max)
        {
            Min = min; Max = max;
        }
        public MinMaxIntConstraint()
        {
            Min = 0; Max = -1; // So it fails.
        }
        public bool IsValid(int value, out string? msg)
        {
            msg = null;
            if (Min <= value && value <= Max)
            {
                return true;
            }
            msg = "Not in range";
            return false;
        }
        public bool IsValid(int? value, out string? msg)
        {
            msg = null;
            return value == null || IsValid(value.Value, out msg);
        }

        public void Read(IConfigurationSection section)
        {
            Min = section.GetValue("min", Min);
            Max = section.GetValue("max", Max);
        }
    }
}
