#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SmartNQuick.AspMvc/SmartNQuick.AspMvc.csproj", "SmartNQuick.AspMvc/"]
COPY ["SmartNQuick.Adapters/SmartNQuick.Adapters.csproj", "SmartNQuick.Adapters/"]
COPY ["SmartNQuick.Logic/SmartNQuick.Logic.csproj", "SmartNQuick.Logic/"]
COPY ["SmartNQuick.Contracts/SmartNQuick.Contracts.csproj", "SmartNQuick.Contracts/"]
COPY ["CommonBase/CommonBase.csproj", "CommonBase/"]
COPY ["SmartNQuick.Transfer/SmartNQuick.Transfer.csproj", "SmartNQuick.Transfer/"]
RUN dotnet restore "SmartNQuick.AspMvc/SmartNQuick.AspMvc.csproj"
COPY . .
WORKDIR "/src/SmartNQuick.AspMvc"
RUN dotnet build "SmartNQuick.AspMvc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartNQuick.AspMvc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartNQuick.AspMvc.dll"]