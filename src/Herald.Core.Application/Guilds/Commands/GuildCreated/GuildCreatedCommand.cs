using Herald.Core.Application.Abstractions;
using Herald.Core.Domain.Entities.Guilds;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Herald.Core.Application.Guilds.Commands.GuildCreated;

public record GuildCreatedCommand : IRequest<ObjectId>
{
    public ulong GuildId { get; init; }
    
    public ulong OwnerId { get; init; }
}

public class GuildCreatedCommandHandler : IRequestHandler<GuildCreatedCommand, ObjectId>
{
    private readonly IHeraldDbContext _context;
    private readonly IDateTime _dateTime;

    public GuildCreatedCommandHandler(IHeraldDbContext context, IDateTime dateTime)
    {
        _context = context;
        _dateTime = dateTime;
    }
    
    public async Task<ObjectId> Handle(GuildCreatedCommand request, CancellationToken cancellationToken)
    {
        var filter = Builders<GuildEntity>
            .Filter.Where(x => x.GuildId.Equals(request.GuildId));
        
        var entity = await _context.Guilds.Find(filter)
            .Limit(1)
            .SingleOrDefaultAsync(cancellationToken);

        if (entity is not null)
        {
            var update = Builders<GuildEntity>.Update
                .Set(x => x.Joined, true)
                .Set(x => x.OwnerId, request.OwnerId)
                .Unset(x => x.LeftOn);

            var updateResult = await _context.Guilds.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

            if (updateResult.ModifiedCount < 1)
            {
                return entity.Id;
            }

            // TODO: This should be an error and not just ignoring it because it didn't update the database
            throw new Exception($"Guild {request.GuildId} failed to update on joining.");
        }
        
        entity = GuildEntity.Create(request.GuildId, request.OwnerId, _dateTime.UtcNow);

        await _context.Guilds.InsertOneAsync(entity, cancellationToken: cancellationToken);

        return entity.Id;
    }
}