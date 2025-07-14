FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ShuppiApi.csproj ./
COPY nuget.config ./

ENV NUGET_PACKAGES=""
RUN dotnet restore ShuppiApi.csproj

COPY . ./
RUN dotnet build ShuppiApi.csproj
