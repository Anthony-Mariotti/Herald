FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Herald.Bot/Herald.Bot.csproj", "Herald.Bot/"]
COPY ["src/Herald.Commands/Herald.Commands.csproj", "Herald.Commands/"]
COPY ["src/Herald.Core/Herald.Core.csproj", "Herald.Core/"]
COPY ["src/Herald.Events/Herald.Events.csproj", "Herald.Events/"]
RUN dotnet restore "src/Herald.Bot/Herald.Bot.csproj"

COPY . .
WORKDIR "/src/Hearald.Bot"
RUN dotnet build "Herald.Bot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Herald.Bot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Herald.Bot.dll"]
