FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BOMA.WTR.Api/BOMA.WTR.Api.csproj", "BOMA.WTR.Api/"]
RUN dotnet restore "BOMA.WTR.Api/BOMA.WTR.Api.csproj"
COPY . .
WORKDIR "/src/BOMA.WTR.Api"
RUN dotnet build "BOMA.WTR.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BOMA.WTR.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BOMA.WTR.Api.dll"]
