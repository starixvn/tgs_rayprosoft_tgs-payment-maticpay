#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BuildConfiguration=Release
RUN --mount=type=secret,id=nuget,dst=/pat cp /pat ~/.nuget/NuGet/NuGet.Config
RUN dotnet nuget remove source 'Microsoft Visual Studio Offline Packages' || true
WORKDIR /src
COPY ["NuGet.config", "."]
COPY ["Tgs.Payment.MaticPay/Tgs.Payment.MaticPay.csproj", "Tgs.Payment.MaticPay/"]
COPY ["MaticPay.Core/MaticPay.Core.csproj", "MaticPay.Core/"]
COPY ["Tgs.Payment.MaticPay.Client/Tgs.Payment.MaticPay.Client.csproj", "Tgs.Payment.MaticPay.Client/"]
COPY ["MaticPay.Repositories/MaticPay.Repositories.csproj", "MaticPay.Repositories/"]
RUN dotnet restore "Tgs.Payment.MaticPay/Tgs.Payment.MaticPay.csproj"
COPY . .
WORKDIR "/src/Tgs.Payment.MaticPay"
RUN dotnet build "Tgs.Payment.MaticPay.csproj" -c ${BuildConfiguration} -o /app/build

FROM build AS publish
ARG BuildConfiguration
RUN dotnet publish "Tgs.Payment.MaticPay.csproj" -c ${BuildConfiguration} -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.Payment.MaticPay.dll"]