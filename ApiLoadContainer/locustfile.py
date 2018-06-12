from locust import HttpLocust, TaskSet, task
import os
from faker import Faker
import random

fake = Faker()

existing_meeting_ids = []

class ApiTasks(TaskSet):
    @task(15)
    def getAllMettings(self):
        self.client.get("/api/meeting",
            name="/api/meeting - get all")
        
    @task(1)
    def addMeeting(self):
        meeting = createFakeMeeting()
        print("Add", meeting)
        response = self.client.post("/api/meeting",
            json = meeting,
            name = "/api/meeting - create")
        existing_meeting_ids.append(extract_meeting_id_from_response(response))

    @task(3)
    def updateMeeting(self):
        if (len(existing_meeting_ids) > 0):
            meeting = createFakeMeeting()
            meeting_id = random.choice(existing_meeting_ids)            
            print("Update", meeting_id, "to", meeting)
            self.client.put("/api/meeting/" + meeting_id,
                json = meeting,
                name = "/api/meeting - update")

class ApiUser(HttpLocust):
    host = os.getenv("api_host")
    task_set = ApiTasks
    min_wait = max_wait = 1000

def extract_value_at_location(content, locator_key, value_length):
    if locator_key in content:
        id_index = content.index(locator_key) + len(locator_key)
        return content[id_index:id_index + value_length]
    raise Exception("Could not find key '{0}' in '{1}'".format(locator_key, content))

def extract_meeting_id_from_response(response):
    headers = response.headers
    location = headers.get("Location")
    result = extract_value_at_location(location, "/meeting/", 36)
    return result

def createFakeMeeting():
    return {
        "title": fake.text(max_nb_chars=100),
        "location": random.choice(['Location1', 'Location5', 'Location13']),
        "description": ' '.join(fake.paragraphs(nb=3)),
        "attendeeLimit": random.choice([None, fake.random_int(min=0, max=150)]),
        "eventDateTime": str(fake.past_date(start_date="-1y", tzinfo=None))
    }
