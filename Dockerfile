#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY *.sln .
COPY Laborie.Service.Api/*.csproj ./Laborie.Service.Api/
COPY Laborie.Service.Application/*.csproj ./Laborie.Service.Application/
COPY Laborie.Service.Domain/*.csproj ./Laborie.Service.Domain/
COPY Laborie.Service.Infrastructure/*.csproj ./Laborie.Service.Infrastructure/
COPY Laborie.Service.Shared/*.csproj ./Laborie.Service.Shared/
RUN dotnet restore 

COPY Laborie.Service.Api/. ./Laborie.Service.Api/
COPY Laborie.Service.Application/. ./Laborie.Service.Application/
COPY Laborie.Service.Domain/. ./Laborie.Service.Domain/
COPY Laborie.Service.Infrastructure/. ./Laborie.Service.Infrastructure/
COPY Laborie.Service.Shared/. ./Laborie.Service.Shared/

WORKDIR /app/Laborie.Service.Api
#
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/sdk:8.0

ENV TZ=Asia/Ho_Chi_Minh
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

#RUN sed -i 's/TLSv1.2/TLSv1.0/g' /etc/ssl/openssl.cnf
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /usr/lib/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /usr/lib/ssl/openssl.cnf

WORKDIR /app
COPY --from=build-env /app/Laborie.Service.Api/out ./
#
ENTRYPOINT ["dotnet", "Laborie.Service.Api.dll"]

