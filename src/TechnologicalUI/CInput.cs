using CompetitiveBackend.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnologicalUI
{
    public static class CInput
    {
        public static string PromtInput(string promt)
        {
            Console.Write(promt);
            return Console.ReadLine()!;
        }
        public static int ReadInt(string promt = "> ")
        {
            return Convert.ToInt32(PromtInput(promt));
        }
        public static string ReadStr(string promt = "> ")
        {
            return PromtInput(promt);
        }
        public static LargeData ReadLD(string promt = "> ")
        {
            return LoadFileDialog(promt);
        }
        public static float ReadFloat(string promt = "> ")
        {
            return (float)Convert.ToDouble(PromtInput(promt));
        }
        public static LargeData LoadFileDialog(string promt)
        {
            string frmt = PromtInput($"{promt}. Введите дирекорию: \n> ");
            try
            {
                byte[] data = File.ReadAllBytes(frmt);
                return new LargeData(data);
            }
            catch(Exception ex)
            {
                throw new FormatException("Could not load file", ex);
            }
        }
    }
}
