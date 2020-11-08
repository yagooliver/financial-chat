FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS base
WORKDIR /app
EXPOSE 5002/tcp

RUN apk add libgdiplus --update-cache --repository http://dl-3.alpinelinux.org/alpine/edge/testing/ --allow-untrusted && \
    apk add terminus-font && \
    apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT false

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build-env
COPY ["./FinancialChat.sln", "./"]
COPY ["./Financial.Chat.Domain.Shared.Bot/Financial.Chat.Domain.Shared.Bot.csproj", "./Financial.Chat.Domain.Shared.Bot/" ]
COPY ["./Financial.Chat.Domain.Shared/Financial.Chat.Domain.Shared.csproj", "./Financial.Chat.Domain.Shared/" ]
COPY ["./Financial.Chat.Web.App/Financial.Chat.Web.App.csproj", "./Financial.Chat.Web.App/" ]
RUN dotnet restore "./Financial.Chat.Web.App/Financial.Chat.Web.App.csproj"
COPY ./ .

RUN dotnet build "./Financial.Chat.Web.App/Financial.Chat.Web.App.csproj" --packages ./.nuget/packages -c Release -o /app/web

FROM build-env AS publish
RUN dotnet publish "./Financial.Chat.Web.App/Financial.Chat.Web.App.csproj" -c Release -o /app/publish


FROM base AS final
WORKDIR /app/web
RUN chmod +x ./

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Financial.Chat.Web.App.dll", "--server.urls", "http://*:5002"]