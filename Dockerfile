#Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /NumberToWordsWebPage

COPY . .

RUN dotnet restore NumberToWordsWebPage.slnx

RUN dotnet publish \
    NumberToWordsWebPage/NumberToWordsWebPage.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish


# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "NumberToWordsWebPage.dll"]