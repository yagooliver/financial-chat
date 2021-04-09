FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS base
WORKDIR /app
EXPOSE 5001/tcp

RUN apk add libgdiplus --update-cache --repository http://dl-3.alpinelinux.org/alpine/edge/testing/ --allow-untrusted && \
    apk add terminus-font && \
    apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT false
#ENV ConnectionStrings:FinancialChatConnection="server=financial-db;database=financial;user=sa;password=dev@1234;convert zero datetime=True;"s

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build-env
COPY ["./FinancialChat.sln", "./"]
COPY ["./Financial.Chat.Domain.Shared.Bot/Financial.Chat.Domain.Shared.Bot.csproj", "./Financial.Chat.Domain.Shared.Bot/" ]
COPY ["./Financial.Chat.Domain.Shared/Financial.Chat.Domain.Shared.csproj", "./Financial.Chat.Domain.Shared/" ]
COPY ["./financial.chat.domain.core/Financial.Chat.Domain.Core.csproj", "./financial.chat.domain.core/" ]
COPY ["./Financial.Chat.Infra.Ioc/Financial.Chat.Infra.Ioc.csproj", "./Financial.Chat.Infra.Ioc/" ]
COPY ["./Financial.Chat.Infra.Bus/Financial.Chat.Infra.Bus.csproj", "./Financial.Chat.Infra.Bus/" ]
COPY ["./Financial.Chat.Infra.Data/Financial.Chat.Infra.Data.csproj", "./Financial.Chat.Infra.Data/" ]
COPY ["./Financial.Chat.Application/Financial.Chat.Application.csproj", "./Financial.Chat.Application/" ]
COPY ["./Financial.Chat.Web.API/Financial.Chat.Web.API.csproj", "./Financial.Chat.Web.API/" ]
RUN dotnet restore "./Financial.Chat.Web.API/Financial.Chat.Web.API.csproj"
COPY ./ .

#RUN dotnet build "./Financial.Chat.Web.API/Financial.Chat.Web.API.csproj" --packages ./.nuget/packages -c Release -o /app/build

#RUN dotnet test

FROM build-env AS publish
RUN dotnet publish "./Financial.Chat.Web.API/Financial.Chat.Web.API.csproj" -c Release -o /app/publish


FROM base AS final
WORKDIR /app/build
RUN chmod +x ./

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Financial.Chat.Web.API.dll", "--server.urls", "http://*:5001"]