from locust import HttpLocust, TaskSet, task
import os

class ApiTasks(TaskSet):    
    def on_start(self):
        self.client.verify = False

    @task(10)
    def getAllMettings(self):
        self.client.get("/api/meeting",
            name="/api/meeting GET ")
        
    @task(1)
    def addMeeting(self):
        self.client.post("/api/meeting",
            json="loadTestMeeting",
            name="/api/meeting POST")

class ApiUser(HttpLocust):
    host = os.getenv("api_host")
    task_set = ApiTasks
    min_wait = max_wait = 1000