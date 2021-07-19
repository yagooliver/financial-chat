# Financial Chat

[![Build status](https://ci.appveyor.com/api/projects/status/rl2ja69994rt3ei6?svg=true)](https://ci.appveyor.com/project/yagooliver/financial-chat)


What is the Financial chat Project?
=====================
Is a simple browser-based chat application using .NET core that use signalR to make the communication with logged users. Also, the user can make calls using the code "/stock=stock_code" to know the stock price.

## Requirements

- If you're running on visual studio you will need the Visual Studio 2019 Version 16.5.3 or High.
- .NET Core SDK 3.1
- The SDK and tools can be downloaded from https://dot.net/core.
- Docker
- Linux or Windows with Hyper-V enable

## Technologies implemented:

- ASP.NET Core 3.1
- ASP.NET WebApi Core with JWT Bearer Authentication
- ASP.NET Identity Core
- ASP.NET CORE SIGNALR
- BLAZOR
- Entity Framework Core 3.1
- .NET Core Native DI
- AutoMapper
- FluentValidator
- MediatR
- Swagger UI with JWT support
- Docker
- RabbitMQ
- MassTransit

## Architecture:

- SOLID
- Domain Driven Design (Layers and Domain Model Pattern)
- Domain Notification
- CQRS
- Unit of Work
- Repository and Generic Repository

## Instructions (using docker)
To run this application you just have to execute the "docker-compose build" command on base directory of the project and then execute "docker-compose up -d". These commands wil run the application and automatically. After running the web api is accessed by http://localhost:8082/swagger and web app by http://localhost:8080

## Instructions (deprecated, available on commit: a24eaf25fe1c26ed1be1147d691f44b7d72b1cd6)

To run this application you just have to execute the "run-script.bat" on base directory of the project. This script will run the dotnet cli commands to run the application and automatically open chrome navigator(normal mode and incognito mode). After running the web api is accessed by https://localhost:44367/swagger and web app by https://localhost:5002

Also, you can run the solution open cmd on base directory and execute the following commands:

- dotnet clean
- dotnet restore
- dotnet build
- dotnet tool install --global dotnet-ef
- dotnet ef database update --startup-project .\Financial.Chat.Web.Api
- start dotnet run --project .\Financial.Chat.Web.API
- start dotnet run --project .\Financial.Chat.Web.App

If you are running in visual studio, follow the steps bellow:
- Run the command "update-database" on Package Manage console with default project Financial.Chat.Infra.Data seted
to create the database
- Start the Financial.Chat.Web.API
- Start The Financial.Chat.Web.App

## Observations
- The Financial.Chat.Web.API must be running using https and port 8082 (old: 44367)
- If web api have been executing in another port, you must change de url port on ChatService class in namespace Financial.Chat.Web.App.Data
- Is necessary to open in two different browsers or in normal mode and other tab in incognito mode since the token for authentication is saved on localstorage for make the requests on Web API
- Make sure that you are using the latest browser version (Chrome, Firefox, Microsoft Edge)
