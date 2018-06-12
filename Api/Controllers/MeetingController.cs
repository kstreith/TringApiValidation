using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Api.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingController : ControllerBase
    {
        private readonly MeetingRepository _meetingRepository;
        
        public MeetingController(MeetingRepository meetingRepository) {
            _meetingRepository = meetingRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Meeting>>> Get()
        {
            var meetingModelResults = await _meetingRepository.GetAllMeetings();
            return meetingModelResults.Select(repoModel => new Meeting {
                Title = repoModel.Title,
                Description = repoModel.Description,
                EventDateTime = repoModel.EventDateTime,
                Location = repoModel.Location,
                AttendeeLimit = repoModel.AttendeeLimit
            }).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Meeting>> Get(string id)
        {
            var meetingModel = await _meetingRepository.GetMeeting(id);
            if (meetingModel == null) {
                return new NotFoundResult();
            }
            var meeting = new Meeting {
                Title = meetingModel.Title,
                Description = meetingModel.Description,
                EventDateTime = meetingModel.EventDateTime,
                Location = meetingModel.Location,
                AttendeeLimit = meetingModel.AttendeeLimit
            };            
            return new ObjectResult(meeting);
        }

        [HttpPost]
        public async Task<ActionResult<MeetingModel>> Post([FromBody] Meeting meeting)
        {
            var newMeetingModel = await _meetingRepository.AddMeeting(new MeetingModel { 
                Title = meeting.Title,
                Description = meeting.Description,
                EventDateTime = meeting.EventDateTime.Value,
                Location = meeting.Location,
                AttendeeLimit = meeting.AttendeeLimit
            });
            var newMeeting = new Meeting {
                Title = newMeetingModel.Title,
                Description = newMeetingModel.Description,
                EventDateTime = newMeetingModel.EventDateTime,
                Location = newMeetingModel.Location,
                AttendeeLimit = newMeetingModel.AttendeeLimit
            };
            return new CreatedResult($"/api/meeting/{newMeetingModel.PrimaryKey}", newMeeting);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MeetingModel>> Put(string id, [FromBody] Meeting meeting)
        {
            var meetingModel = await _meetingRepository.GetMeeting(id);
            if (meetingModel == null) {
                return new NotFoundResult();
            }            
            var newMeetingModel = new MeetingModel { 
                Title = meeting.Title,
                Description = meeting.Description,
                EventDateTime = meeting.EventDateTime.Value,
                Location = meeting.Location,
                AttendeeLimit = meeting.AttendeeLimit
            };
            var updatedMeetingModel = await _meetingRepository.UpdateMeeting(id, newMeetingModel);
            var updatedMeeting = new Meeting {
                Title = updatedMeetingModel.Title,
                Description = updatedMeetingModel.Description,
                EventDateTime = updatedMeetingModel.EventDateTime,
                Location = updatedMeetingModel.Location,
                AttendeeLimit = updatedMeetingModel.AttendeeLimit
            };            
            return new OkObjectResult(updatedMeeting);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var meetingModel = await _meetingRepository.GetMeeting(id);
            if (meetingModel == null) {
                return new NotFoundResult();
            }            
            await _meetingRepository.DeleteMeeting(id);
            return new OkResult();
        }
    }
}
