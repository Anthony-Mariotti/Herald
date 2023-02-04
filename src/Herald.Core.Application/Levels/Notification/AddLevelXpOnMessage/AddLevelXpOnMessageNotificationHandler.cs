using Herald.Core.Application.Abstractions;
using Herald.Core.Application.Events.Notification.MessageCreated;
using Herald.Core.Application.Exceptions;
using Herald.Core.Domain.Entities.Guilds;
using Herald.Core.Domain.Entities.Leveling;
using Herald.Core.Domain.Entities.Modules;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Herald.Core.Application.Levels.Notification.AddLevelXpOnMessage;

public class AddLevelXpOnMessageNotificationHandler : INotificationHandler<MessageCreatedNotification>
{
    private readonly IHeraldDbContext _context;

    public AddLevelXpOnMessageNotificationHandler(IHeraldDbContext context)
    {
        _context = context;
    }

    public async Task Handle(MessageCreatedNotification notification, CancellationToken cancellationToken)
    {
        var guild = await _context.Guilds
            .Include(x => x.Modules)
            .SingleOrDefaultAsync(x => x.Id.Equals(notification.EventArgs.Guild.Id), cancellationToken);

        if (guild == null)
        {
            throw new NotFoundException(nameof(Guild), notification.EventArgs.Guild.Id);
        }

        if (!guild.HasAccess(Module.Economy))
        {
            return;
        }

        var leveling = await _context.Levels
            .Include(x => x.Members)
            .SingleOrDefaultAsync(x => x.GuildId.Equals(notification.EventArgs.Guild.Id), cancellationToken);

        if (leveling == null)
        {
            leveling = new Level(
                notification.EventArgs.Guild.Id,
                false, false, false, 1.0,
                new(), new(), new(), new());

            _ = _context.Levels.Add(leveling);
        }

        leveling.RewardXp(notification.EventArgs.Author.Id);

        _ = await _context.SaveChangesAsync(cancellationToken);
    }
}