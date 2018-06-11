Run Locally
-----------
```
docker run --rm -v ${PWD}:/locust -p 8089:8089 -e api_host=https://trinug-api-validation.azurewebsites.net kstreith/locust
```