Run on command-line using Newman & Docker
-----------------------------------------
1. Open Powershell 
2. Fetch Docker Image
```
docker pull postman/newman_alpine33
```
3. Run newman with Docker
```
docker run -v ${PWD}:/etc/newman -t postman/newman_alpine33 run "TrinugApiValidation.postman_collection.json" --environment="Local.postman_environment.json" --reporters="html,cli" --reporter-html-export="newman-results.html"
```