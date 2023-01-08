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

#--allow-unauthenticated
RUN apt-get update && apt-get install -y libgdiplus
#RUN apt-get update \
#    && apt-get install -y --no-install-recommends \
#        zlib1g \
#        fontconfig \
#        libfreetype6 \
#        libx11-6 \
#        libxext6 \
#        libxrender1 \
#        libjpeg62-turbo

        #ca-certificates \
        #xvfb \
        #libfontconfig \
        #wkhtmltopdf \
        #fontconfig \
        #zlib1g \
        #fontconfig \
        #libfreetype6 \
        #libx11-6 \
        #libxext6 \
        #libxrender1 \
        #libjpeg62-turbo \
        #libxcb1 \
        #xfonts-75dpi \
        #xfonts-base \
        #openssl \
        #wget \
        #gdebi


#RUN apt-get install -y apt-utils
#RUN apt-get install -y zlib1g
#RUN apt-get install -y fontconfig
#RUN apt-get install -y libc6-dev
#RUN apt-get install -y libxrender1
#RUN apt-get install -y libfontconfig1
#RUN apt-get install -y libxext-dev
#RUN ln -s /usr/lib/libgdiplus.so/usr/lib/gdiplus.dll


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Quiz.Api.dll"]