#! /bin/sh

# First parameter is build mode, defaults to Debug
MODE=${1:-Debug}

# Find the solution file in the root take it's name
NAME=$(basename $(ls *.sln | head -n 1) .sln)

if [ -x "$(command -v docker)" ]; then
  CONTAINER=$(docker run -d --rm -p 8600:8600 -p 8500:8500 -p 8300:8300 -e CONSUL_BIND_INTERFACE=eth0 consul)
  echo "Started Consul container: $CONTAINER"
  sleep 2
fi

dotnet restore
dotnet build --configuration $MODE

find ./src -iname "*.Tests.csproj" -type f -exec dotnet test --no-build "{}" --configuration $MODE \;

dotnet pack --no-build --configuration $MODE --output ../../.build

if [ -x "$(command -v docker)" ]; then
  docker stop $CONTAINER &> /dev/null
  echo "Removed Consul container"
fi
