FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:35792ea4ad1db051981f62b313f1be3b46b1f45cadbaa3c288cd0d3056eefb83 AS build-env
WORKDIR /todo-tasks-api/TodoAPI.API
EXPOSE 8080 8081

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore TodoAPI.sln
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0@sha256:6c4df091e4e531bb93bdbfe7e7f0998e7ced344f54426b7e874116a3dc3233ff
WORKDIR /todo-tasks-api/TodoAPI.API
COPY --from=build-env /todo-tasks-api/TodoAPI.API/out .
ENTRYPOINT ["dotnet", "./TodoAPI.API.dll"]