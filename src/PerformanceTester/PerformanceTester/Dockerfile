#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/sdk:3.1
WORKDIR /app
COPY . /app

RUN dotnet restore
RUN dotnet build

WORKDIR /app
ENV ASPNETCORE_URLS http://*:5000,https://*:5001
ENV ASPNETCORE_HTTPS_PORT 5001
ENV ASPNETCORE_Kestrel__Certificates__Default__Password "P@ssw0rd"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path "./Https/docker.pfx"
ENV ASPNETCORE_ENVIRONMENT docker
EXPOSE 5000 5001

ENTRYPOINT dotnet run
