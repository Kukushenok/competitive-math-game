
global using IDType = int;
namespace CompetitiveBackend.Core.Objects
{
    public abstract class Identifiable<T> where T : struct
    {
        public readonly T? Id = null;
        public Identifiable(T? id = null)
        {
            Id = id;
        }
    }
    /// <summary>
    /// Базовый класс для объектов, поддерживающие идентификацию по целому числу
    /// </summary>
    public abstract class IntIdentifiable : Identifiable<IDType>
    {
        public IntIdentifiable(IDType? id = null) : base(id) { }
    }
}
