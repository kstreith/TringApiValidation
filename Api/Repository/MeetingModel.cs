using System;
using Newtonsoft.Json;

namespace Api.Repository
{
    public class MeetingModel {
        [JsonProperty(PropertyName = "id")]
        public string Id => $"{PrimaryKey}-Meeting";

        public string PrimaryKey { get; set; }

        public string Title { get; set; }

        public DateTimeOffset EventDateTime { get; set; }

        public string Description { get; set; }

        public int? AttendeeLimit { get; set; }

        public string Location { get; set; }
    }
}