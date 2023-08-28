FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["rabbitmq.merge.sort.producer/rabbitmq.merge.sort.producer.csproj", "rabbitmq.merge.sort.producer/"]
RUN dotnet restore "rabbitmq.merge.sort.producer/rabbitmq.merge.sort.producer.csproj"
COPY . .
WORKDIR "/src/rabbitmq.merge.sort.producer"
RUN dotnet build "rabbitmq.merge.sort.producer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "rabbitmq.merge.sort.producer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "rabbitmq.merge.sort.producer.dll"]
