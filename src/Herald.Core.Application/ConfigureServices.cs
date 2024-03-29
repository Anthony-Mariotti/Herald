﻿using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Herald.Core.Application.Common.Behavior;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Herald.Core.Application;

public static class ConfigureServices
{
    [SuppressMessage("Style", "IDE0058:Expression value is never used", Justification = "Readability")]
    public static IServiceCollection AddHeraldApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));

        return services;
    }
}