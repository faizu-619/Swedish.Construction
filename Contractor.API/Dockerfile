#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Contractor.API/Contractor.API.csproj", "Contractor.API/"]
RUN dotnet restore "Contractor.API/Contractor.API.csproj"
COPY . .
WORKDIR "/src/Contractor.API"
RUN dotnet build "Contractor.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Contractor.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Contractor.API.dll"]