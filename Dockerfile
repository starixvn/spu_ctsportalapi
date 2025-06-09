FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder

WORKDIR /src

COPY ["./src/Fanex.NAP.CTS.Core", "Fanex.NAP.CTS.Core/"]

COPY ["./src/Fanex.NAP.CTS.Criteria", "Fanex.NAP.CTS.Criteria/"]

COPY ["./src/Fanex.NAP.CTS.Model", "Fanex.NAP.CTS.Model/"]

COPY ["./src/Fanex.NAP.CTS.Resource", "Fanex.NAP.CTS.Resource/"]

COPY ["./src/Fanex.NAP.CTS.Service", "Fanex.NAP.CTS.Service/"]

COPY ["./src/Fanex.NAP.CTS.ViewModel", "Fanex.NAP.CTS.ViewModel/"]

COPY ["./src/Fanex.NAP.CTS.PortalAPI/Fanex.NAP.CTS.PortalAPI.csproj", "Fanex.NAP.CTS.PortalAPI/"]

WORKDIR "/src/Fanex.NAP.CTS.PortalAPI"

RUN dotnet nuget add source http://nuget.nexdev.net/api/v2

RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore -v n -r linux-x64

COPY ./src/Fanex.NAP.CTS.PortalAPI .

RUN dotnet publish -r linux-x64 --self-contained false -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine AS base

WORKDIR /app

COPY --from=builder /app .

ENTRYPOINT ["dotnet", "Fanex.NAP.CTS.PortalAPI.dll"]

ENV TZ="America/La_Paz"