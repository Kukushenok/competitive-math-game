using System.Security.Cryptography;

namespace CompetitiveBackend.Services.Objects
{
    public interface IHashAlgorithm
    {
        /// <summary>
        /// Проверка строки с соответствующим хэшом
        /// </summary>
        /// <param name="input">Строка</param>
        /// <param name="hash">Исходный хэш</param>
        /// <returns>Соответствие хэша строки и исходного хэша</returns>
        public bool Verify(string input, string hash);
        /// <summary>
        /// Выполняет операцию хэширования
        /// </summary>
        /// <param name="input">Входная строка (например, пароль)</param>
        /// <returns>Хэш строки</returns>
        public string Hash( string input );
    }
}
