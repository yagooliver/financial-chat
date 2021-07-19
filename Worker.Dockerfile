FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine3.13-amd64 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine3.13-amd64 AS build-env
COPY ["./FinancialChat.sln", "./"]
COPY ["./Financial.Chat.Domain.Shared/Financial.Chat.Domain.Shared.csproj", "./Financial.Chat.Domain.Shared/" ]
COPY ["./Financial.Chat.Infra.Identity/Financial.Chat.Infra.Identity.csproj", "./Financial.Chat.Infra.Identity/"]
COPY ["./FinancialChat.MessageBroker/FinancialChat.MessageBroker.csproj", "./FinancialChat.MessageBroker/"]
#RUN dotnet restore "./Financial.Chat.Web.API/Financial.Chat.Web.API.csproj"
COPY ./ .

#RUN dotnet build "./Financial.Chat.Web.API/Financial.Chat.Web.API.csproj" --packages ./.nuget/packages -c Release -o /app/build

#RUN dotnet test

FROM build-env AS publish
RUN dotnet publish "./FinancialChat.MessageBroker/FinancialChat.MessageBroker.csproj" -c Production -o /app/publish


FROM base AS final
WORKDIR /app/build
RUN chmod +x ./

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "FinancialChat.MessageBroker.dll"]