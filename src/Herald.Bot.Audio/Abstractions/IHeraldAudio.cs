using DSharpPlus.SlashCommands;
using Herald.Core.Domain.ValueObjects.Soundtracks;
using Lavalink4NET.Player;
using Lavalink4NET.Rest;

namespace Herald.Bot.Audio.Abstractions;

public interface IHeraldAudio
{
    // Search Track
    public Task<LavalinkTrack?> SearchAsync(InteractionContext context, string search, SearchMode mode);

    // Play Track
    public Task PlayAsync(InteractionContext context, string search, SearchMode mode);

    // Skip Track
    public Task SkipAsync(InteractionContext context);
    
    // Enqueue Track
    public Task EnqueueAsync(InteractionContext context, string search, SearchMode mode);
    public Task EnqueueAsync(InteractionContext context, LavalinkTrack track, bool queueOnly = false);

    // Dequeue Track
    public Task DequeueAsync(InteractionContext context, QueuedTrackValue track);

    // List Queue
    public Task GetQueueAsync(InteractionContext context);

    // Start Queue
    public Task PlayQueueAsync(InteractionContext context);

    // Stop Playing
    public Task StopAsync(InteractionContext context);

    // Pause Playing
    public Task PauseAsync(InteractionContext context);
    
    // Resume Playing
    public Task ResumeAsync(InteractionContext context);
}