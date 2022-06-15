using MongoDB.Bson;

namespace Herald.Core.Domain.Common;

public class BaseEntity
{
    public ObjectId Id { get; set; }
}