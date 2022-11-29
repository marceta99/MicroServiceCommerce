
#this docker file consits of two parts, first is for building the app, second is for publishing and runing app
#building part 
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base #here we use Net3.1 image as base image 
WORKDIR /app #working directory is the folder in which will docker copy the files from our project and that is app folder
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Services/Catalog/Catalog.API/Catalog.API.csproj", "Services/Catalog/Catalog.API/"] 
RUN dotnet restore "Services/Catalog/Catalog.API/Catalog.API.csproj"
COPY . .
WORKDIR "/src/Services/Catalog/Catalog.API"
RUN dotnet build "Catalog.API.csproj" -c Release -o /app/build

#copy command will copy project files from local file system to docker image


#publishing part
FROM build AS publish
RUN dotnet publish "Catalog.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]

#entrypoint means that when container is up and running first thing that will be run is Catalog.Api.dll
