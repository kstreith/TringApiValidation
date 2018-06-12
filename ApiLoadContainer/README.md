Run Locally
-----------
1. Open powershell, change directory to location of this READM

2. Run locust using Docker command
```
docker run --rm -v ${PWD}:/locust -p 8089:8089 -e api_host=https://trinug-api-validation.azurewebsites.net kstreith/locust
```

3. Open browser to http://localhost:8089/ to view the Locust interface
4. Enter 10 for both numeric entries to start the test.

Create Azure Container Instance
-------------------------------
1. Open Azure Cloud Shell once logged into: http://portal.azure.com/

2. Execute commands in Powershell version of Azure Cloud Shell
3. Create Resource Group
```
New-AzureRmResourceGroup -Name trinugApiLoad -Location EastUS
```
4. Create Container Instance in East US
```
New-AzureRmContainerGroup -ResourceGroupName trinugApiLoad -Name trinug-api-load-eus1 -Image kstreith/api-validation-load:latest -OsType Linux -Port 8089 -DnsNameLabel trinug-api-load-eus1
```
5. Check on status of creation, looking for 'Succeeded'
```
Get-AzureRmContainerGroup -ResourceGroupName trinugApiLoad -Name trinug-api-load-eus1
```

6. Create Container Instance in West US
```
New-AzureRmContainerGroup -ResourceGroupName trinugApiLoad -Name trinug-api-load-weur1 -Image kstreith/api-validation-load:latest -OsType Linux -Port 8089 -Location WestEurope -DnsNameLabel trinug-api-load-weur1
```

7. Check on status of creation, looking of 'Succeeded'
```
Get-AzureRmContainerGroup -ResourceGroupName trinugApiLoad -Name trinug-api-load-weur1
```

8. Open browser to location of running container instance, port 8089.
9. Enter 10 in both numeric boxes to have container instance start driving traffic at endpoint using Locust.