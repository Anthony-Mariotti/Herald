﻿using FluentValidation;
using Herald.Core.Application.Soundtracks.Commands.QueueTrack;

namespace Herald.Core.Application.Soundtracks.Commands.AddTrackToQueue;

public class QueueTrackCommandValidator : AbstractValidator<QueueTrackCommand>
{
    public QueueTrackCommandValidator()
    {
        RuleFor(x => x.GuildId)
            .NotEmpty().WithMessage("GuildId is required.");

        RuleFor(x => x.NotifyChannelId)
            .NotEmpty().WithMessage("NotifyChannelId is required.");

        RuleFor(x => x.RequestUserId)
            .NotEmpty().WithMessage("RequestUserId is required.");
        
        RuleFor(x => x.Track)
            .NotEmpty().WithMessage("Track is required.");
    }
}