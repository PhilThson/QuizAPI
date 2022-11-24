#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Quiz.Api/Quiz.Api.csproj", "Quiz.Api/"]
COPY ["Quiz.Data/Quiz.Data.csproj", "Quiz.Data/"]
COPY ["Quiz.Infrastructure/Quiz.Infrastructure.csproj", "Quiz.Infrastructure/"]
COPY ["Quiz.Shared/Quiz.Shared.csproj", "Quiz.Shared/"]
RUN dotnet restore "Quiz.Api/Quiz.Api.csproj"
COPY . .
WORKDIR "/src/Quiz.Api"
RUN dotnet build "Quiz.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Quiz.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Quiz.Api.dll"]