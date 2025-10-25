namespace CompetitiveBackend.Core.Objects
{
    public abstract class Identifiable<T>
        where T : struct
    {
        public readonly T? Id;
        public Identifiable(T? id = null)
        {
            Id = id;
        }
    }

    /// <summary>
    /// Базовый класс для объектов, поддерживающие идентификацию по целому числу.
    /// </summary>
    public abstract class IntIdentifiable : Identifiable<int>
    {
        public IntIdentifiable(int? id = null)
            : base(id)
        {
        }
    }
}
