# Local Development
## Log into Azure Container Registry
az login -t d9f3dee8-148c-49ea-8e87-dd97cd0cd5de
az account set -s 7a9a0f8c-eb0d-4803-89f5-4e9e32a6333d
az acr login -n dxpprivate

## Use Build Cake to run docker container
```powershell
$env:CAKE_SETTINGS_SKIPPACKAGEVERSIONCHECK="true"
.\build.ps1 --target=Compose
.\build.ps1 --target=Up
.\build.ps1 --target=Down
```

## Run Unit Test
newman run -k -e ./tests/IntegrationTest/AppData/Docker.postman_environment.json ./tests/IntegrationTest/AppData/Test.postman_collection.json

# RabbitMQ
$trackingEndpoint = 'https://ydx-test01-ppm-be-sea-wa.azurewebsites.net/fnc/mst/messaging/rabbitmq?code=WTMeQzpGzY9T2n8sK4HHDHvUhtzYepuJfqxvFuWJ2yJnWbee7X2u85HWGTV'
Invoke-WebRequest $trackingEndpoint -UseBasicParsing | Set-Content './rabbitmq/rabbitmq-definitions.json'