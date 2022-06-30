#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["src/Herald.Bot/Herald.Bot.csproj", "src/Herald.Bot/"]
COPY ["src/Herald.Bot.Audio/Herald.Bot.Audio.csproj", "src/Herald.Bot.Audio/"]
COPY ["src/Herald.Bot.Commands/Herald.Bot.Commands.csproj", "src/Herald.Bot.Commands/"]
COPY ["src/Herald.Bot.Events/Herald.Bot.Events.csproj", "src/Herald.Bot.Events/"]

COPY ["src/Herald.Core/Herald.Core.csproj", "src/Herald.Core/"]
COPY ["src/Herald.Core.Application/Herald.Core.Application.csproj", "src/Herald.Core.Application/"]
COPY ["src/Herald.Core.Domain/Herald.Core.Domain.csproj", "src/Herald.Core.Domain/"]
COPY ["src/Herald.Core.Infrastructure/Herald.Core.Infrastructure.csproj", "src/Herald.Core.Infrastructure/"]

RUN dotnet restore "src/Herald.Bot/Herald.Bot.csproj"
COPY . .

WORKDIR "/src/src/Herald.Bot"
RUN dotnet build "Herald.Bot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Herald.Bot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Herald.Bot.dll"]