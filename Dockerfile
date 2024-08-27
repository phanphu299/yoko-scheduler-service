# SDK build image
FROM dxpprivate.azurecr.io/ahi-build:6.0 AS build
WORKDIR .
COPY NuGet.Config ./
COPY src/Scheduler.Api/*.csproj         ./src/Scheduler.Api/
COPY src/Scheduler.Application/*.csproj ./src/Scheduler.Application/
COPY src/Scheduler.Domain/*.csproj      ./src/Scheduler.Domain/
COPY src/Scheduler.Persistence/*.csproj ./src/Scheduler.Persistence/
RUN dotnet restore ./src/Scheduler.Api/*.csproj /property:Configuration=Release -nowarn:msb3202,nu1503

COPY src/ ./src
RUN dotnet publish ./src/Scheduler.Api/*.csproj --no-restore -c Release -o /app/out

# Run time image
FROM dxpprivate.azurecr.io/ahi-runtime:6.0-full as final
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Scheduler.Api.dll"]