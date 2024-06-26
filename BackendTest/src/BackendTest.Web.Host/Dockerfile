FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY ["src/BackendTest.Web.Host/BackendTest.Web.Host.csproj", "src/BackendTest.Web.Host/"]
COPY ["src/BackendTest.Web.Core/BackendTest.Web.Core.csproj", "src/BackendTest.Web.Core/"]
COPY ["src/BackendTest.Application/BackendTest.Application.csproj", "src/BackendTest.Application/"]
COPY ["src/BackendTest.Core/BackendTest.Core.csproj", "src/BackendTest.Core/"]
COPY ["src/BackendTest.EntityFrameworkCore/BackendTest.EntityFrameworkCore.csproj", "src/BackendTest.EntityFrameworkCore/"]
WORKDIR "/src/src/BackendTest.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/BackendTest.Web.Host", "src/BackendTest.Web.Host"]
COPY ["src/BackendTest.Web.Core", "src/BackendTest.Web.Core"]
COPY ["src/BackendTest.Application", "src/BackendTest.Application"]
COPY ["src/BackendTest.Core", "src/BackendTest.Core"]
COPY ["src/BackendTest.EntityFrameworkCore", "src/BackendTest.EntityFrameworkCore"]
WORKDIR "/src/src/BackendTest.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "BackendTest.Web.Host.dll"]
