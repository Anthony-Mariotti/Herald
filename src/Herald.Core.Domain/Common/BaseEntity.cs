namespace Herald.Core.Domain.Common;

public class BaseEntity : BaseEntity<long> { }

public class BaseEntity<T> where T : struct
{
    public T Id { get; set; }
}