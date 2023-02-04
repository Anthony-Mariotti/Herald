using Herald.Core.Domain.ValueObjects.Soundtracks;
using MediatR;

namespace Herald.Core.Application.Soundtracks.Commands.DequeueTrack;

public record DequeueTrackCommand(ulong GuildId, QueuedTrackValue Track) : IRequest;

public class DequeueTrackCommandHandler : IRequestHandler<DequeueTrackCommand>
{
    public Task<Unit> Handle(DequeueTrackCommand request, CancellationToken cancellationToken) =>
        throw new NotImplementedException();
}