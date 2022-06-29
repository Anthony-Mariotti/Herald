﻿using FluentValidation;
using Herald.Core.Domain.Enums;

namespace Herald.Core.Application.Soundtracks.Commands.TrackEnded;

public class TrackEndedCommandValidator : AbstractValidator<TrackEndedCommand>
{
    public TrackEndedCommandValidator()
    {
        RuleFor(x => x.GuildId)
            .NotEmpty().WithMessage($"{nameof(TrackEndedCommand)} requires a GuildId");
        
        RuleFor(x => x.Identifier)
            .NotEmpty().WithMessage($"{nameof(TrackEndedCommand)} requires an Identifier");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage($"{nameof(TrackEndedCommand)} requires a Reason")
            .Must(HaveValidReason).WithMessage((_, reason) => $"\"{reason.Name}\" is not a valid reason.");
    }

    private static bool HaveValidReason(TrackStatusReason reason)
        => TrackStatusReason.List.Contains(reason);
}