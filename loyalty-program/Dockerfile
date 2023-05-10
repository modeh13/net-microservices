FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["LoyaltyProgram/LoyaltyProgram.csproj", "LoyaltyProgram/"]
RUN dotnet restore "LoyaltyProgram/LoyaltyProgram.csproj"
COPY . .
WORKDIR "/src/LoyaltyProgram"
RUN dotnet build "LoyaltyProgram.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LoyaltyProgram.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LoyaltyProgram.dll"]
