# Build ClientApp
FROM node:16-alpine AS react-build-env
WORKDIR /app
COPY ./ClientApp/ ./
RUN npm install
RUN npm run build

# Build dotnet application
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
COPY --from=react-build-env /app/build ./ClientApp/build
ENTRYPOINT ["dotnet", "JourneyPlanner.dll"]
