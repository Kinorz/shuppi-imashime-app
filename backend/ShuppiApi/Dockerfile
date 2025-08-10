# ===== runtime =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
# Render/他PaaSが注入する PORT を使う。ローカルは8080デフォルト。
ARG PORT=8080
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}
# 逆プロキシ配下でのURL判定を安定させる
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
EXPOSE 8080

# ===== build =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ShuppiApi.csproj ./
COPY nuget.config ./
RUN dotnet restore ShuppiApi.csproj
COPY . .
# 実行ファイルだけを吐き出す（UseAppHost=false で軽量化）
RUN dotnet publish ShuppiApi.csproj -c Release -o /out /p:UseAppHost=false

# ===== final =====
FROM base AS final
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet","ShuppiApi.dll"]
