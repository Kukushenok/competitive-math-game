namespace TechnologicalUIHost.ConsoleAbstractions
{
    public static class ConsoleInputExtensions
    {
        public static int ReadInt(this IConsoleInput input, string promt = "> ")
        {
            return Convert.ToInt32(input.PromtInput(promt));
        }

        public static float ReadFloat(this IConsoleInput input, string promt = "> ")
        {
            return (float)Convert.ToDouble(input.PromtInput(promt));
        }

        public static DateTime ReadDateTime(this IConsoleInput input, string promt = "> ")
        {
            return Convert.ToDateTime(input.PromtInput(promt));
        }

        public static int? ReadNullableInt(this IConsoleInput input, string promt = "> ")
        {
            string inputStr = input.PromtInput(promt);
            return string.IsNullOrWhiteSpace(inputStr) ? null : Convert.ToInt32(inputStr);
        }

        public static float? ReadNullableFloat(this IConsoleInput input, string promt = "> ")
        {
            string inputStr = input.PromtInput(promt);
            return string.IsNullOrWhiteSpace(inputStr) ? null : (float)Convert.ToDouble(inputStr);
        }

        public static DateTime? ReadNullableDateTime(this IConsoleInput input, string promt = "> ")
        {
            string inputStr = input.PromtInput(promt);
            return string.IsNullOrWhiteSpace(inputStr) ? null : Convert.ToDateTime(inputStr);
        }

        public static TimeSpan? ReadNullableTimeSpan(this IConsoleInput input, string promt = "> ")
        {
            string inputStr = input.PromtInput(promt);
            return string.IsNullOrWhiteSpace(inputStr) ? null : TimeSpan.Parse(inputStr);
        }
    }
}
