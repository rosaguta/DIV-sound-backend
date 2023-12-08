FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DIV-SOUND-backend/DIV-SOUND-backend.csproj", "DIV-SOUND-backend/"]
RUN dotnet restore "DIV-SOUND-backend/DIV-SOUND-backend.csproj"
COPY . .
WORKDIR "/src/DIV-SOUND-backend"
RUN dotnet build "DIV-SOUND-backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DIV-SOUND-backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DIV-SOUND-backend.dll"]
