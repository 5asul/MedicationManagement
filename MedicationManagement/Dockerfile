#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MedicationManagement/MedicationManagement.csproj", "MedicationManagement/"]
RUN dotnet restore "./MedicationManagement/./MedicationManagement.csproj"
COPY . .
WORKDIR "/src/MedicationManagement"
RUN dotnet build "./MedicationManagement.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MedicationManagement.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MedicationManagement.dll"]

# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Install globalization libraries (ICU)
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    icu-devtools libicu-dev \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

# Set environment variable to disable globalization-invariant mode
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Set default locale to en-US
ENV LANG=en_US.UTF-8
ENV LC_ALL=en_US.UTF-8

# Set the working directory in the container
WORKDIR /app

# Copy all files from the current directory to the app folder in the container
COPY . .

# Set the entry point for the container to run your application
ENTRYPOINT ["dotnet", "MedicationManagement.dll"]

