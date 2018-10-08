FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ./backend/Demo.WebApi/Demo.WebApi.csproj ./backend/Demo.WebApi/
COPY ./backend/Demo.Shared/Demo.Shared.csproj ./backend/Demo.Shared/
COPY ./backend/Demo.Data/Demo.Data.csproj ./backend/Demo.Data/
RUN dotnet restore ./backend/Demo.WebApi/Demo.WebApi.csproj

COPY ./backend ./backend
WORKDIR /src/backend/Demo.WebApi
RUN dotnet build Demo.WebApi.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Demo.WebApi.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Demo.WebApi.dll"]
