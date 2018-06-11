
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace Api.Repository
{
    public class MeetingRepository
    {
        private const string DbName = "MainDb";
        private const string CollectionName = "TrinugApiValidation";
        private DocumentClient client;        

        public MeetingRepository(CosmosConfig cosmosConfig)
        {
            client = new DocumentClient(new Uri(cosmosConfig.EndpointUrl), cosmosConfig.AuthKey);
        }

        private async Task CreateIfNeeded()
        {
            await client.CreateDatabaseIfNotExistsAsync(new Database { Id = DbName });
            await client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(DbName),
                new DocumentCollection { Id = CollectionName });
        }

        public virtual async Task<MeetingModel> AddMeeting(MeetingModel meeting)
        {
            await CreateIfNeeded();
            var dcUri = UriFactory.CreateDocumentCollectionUri(DbName, CollectionName);
            meeting.PrimaryKey = Guid.NewGuid().ToString();
            await client.CreateDocumentAsync(dcUri, meeting);
            return meeting;
        }

        public virtual async Task<MeetingModel> GetMeeting(string id)
        {
            await CreateIfNeeded();
            var dcUri = UriFactory.CreateDocumentUri(DbName, CollectionName, $"{id}-Meeting");
            try
            {
                var meeting = await client.ReadDocumentAsync<MeetingModel>(dcUri);
                return meeting;
            }
            catch (DocumentClientException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public virtual async Task DeleteMeeting(string id)
        {
            await CreateIfNeeded();
            var dcUri = UriFactory.CreateDocumentUri(DbName, CollectionName, $"{id}-Meeting");
            await client.DeleteDocumentAsync(dcUri);
        }

        public virtual async Task UpdateMeeting(string id, MeetingModel meeting)
        {
            await CreateIfNeeded();
            var dcUri = UriFactory.CreateDocumentUri(DbName, CollectionName, $"{id}-Meeting");
            await client.ReplaceDocumentAsync(dcUri, meeting);
        }

        public virtual async Task<List<MeetingModel>> GetAllMeetings()
        {
            await CreateIfNeeded();
            var dcUri = UriFactory.CreateDocumentCollectionUri(DbName, CollectionName);
            var queryOptions = new FeedOptions { MaxItemCount = 100 };
            var meetingQuery = client.CreateDocumentQuery<MeetingModel>(dcUri, queryOptions);
            var asyncMeetingQuery = meetingQuery.AsDocumentQuery();
            var asyncMeetingQueryResult = await asyncMeetingQuery.ExecuteNextAsync<MeetingModel>();
            return asyncMeetingQueryResult.ToList();
        }
    }
}