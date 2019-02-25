using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FaceDemo.Misc;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;

namespace FaceDemo.FaceApi
{
    public class AutoRetryFaceClient : IFaceServiceClient
    {
        private readonly IFaceServiceClient innerClient;

        public AutoRetryFaceClient(IFaceServiceClient innerClient)
        {
            this.innerClient = innerClient;
        }

        public Task<AddPersistedFaceResult> AddFaceToFaceListAsync(string faceListId, string imageUrl, string userData = null, FaceRectangle targetFace = null)
        {
            return innerClient.AddFaceToFaceListAsync(faceListId, imageUrl, userData, targetFace);
        }

        public Task<AddPersistedFaceResult> AddFaceToFaceListAsync(string faceListId,
            Stream imageStream,
            string userData = null,
            FaceRectangle targetFace = null)
        {
            return innerClient.AddFaceToFaceListAsync(faceListId, imageStream, userData, targetFace);
        }

        public Task<AddPersistedFaceResult> AddPersonFaceAsync(string personGroupId,
            Guid personId,
            string imageUrl,
            string userData = null,
            FaceRectangle targetFace = null)
        {
            return innerClient.AddPersonFaceAsync(personGroupId, personId, imageUrl, userData, targetFace);
        }

        public Task<AddPersistedFaceResult> AddPersonFaceAsync(string personGroupId,
            Guid personId,
            Stream imageStream,
            string userData = null,
            FaceRectangle targetFace = null)
        {
            return innerClient.AddPersonFaceAsync(personGroupId, personId, imageStream, userData, targetFace);
        }

        public Task CreateFaceListAsync(string faceListId, string name, string userData)
        {
            return innerClient.CreateFaceListAsync(faceListId, name, userData);
        }

        public Task<CreatePersonResult> CreatePersonAsync(string personGroupId, string name, string userData = null)
        {
            return innerClient.CreatePersonAsync(personGroupId, name, userData);
        }

        public Task CreatePersonGroupAsync(string personGroupId, string name, string userData = null)
        {
            return innerClient.CreatePersonGroupAsync(personGroupId, name, userData);
        }

        public Task DeleteFaceFromFaceListAsync(string faceListId, Guid persistedFaceId)
        {
            return innerClient.DeleteFaceFromFaceListAsync(faceListId, persistedFaceId);
        }

        public Task DeleteFaceListAsync(string faceListId)
        {
            return innerClient.DeleteFaceListAsync(faceListId);
        }

        public Task DeletePersonAsync(string personGroupId, Guid personId)
        {
            return innerClient.DeletePersonAsync(personGroupId, personId);
        }

        public Task DeletePersonFaceAsync(string personGroupId, Guid personId, Guid persistedFaceId)
        {
            return innerClient.DeletePersonFaceAsync(personGroupId, personId, persistedFaceId);
        }

        public Task DeletePersonGroupAsync(string personGroupId)
        {
            return innerClient.DeletePersonGroupAsync(personGroupId);
        }

        public Task<Microsoft.ProjectOxford.Face.Contract.Face[]> DetectAsync(string imageUrl, bool returnFaceId, bool returnFaceLandmarks, IEnumerable<FaceAttributeType> returnFaceAttributes = null)
        {
            return RateLimitAwareCall(() => innerClient.DetectAsync(imageUrl, returnFaceId, returnFaceLandmarks, returnFaceAttributes));
        }

        public Task<Microsoft.ProjectOxford.Face.Contract.Face[]> DetectAsync(Stream imageStream,
            bool returnFaceId,
            bool returnFaceLandmarks,
            IEnumerable<FaceAttributeType> returnFaceAttributes = null)
        {
            return RateLimitAwareCall(() => innerClient.DetectAsync(imageStream, returnFaceId, returnFaceLandmarks, returnFaceAttributes));
        }

        public Task<SimilarFace[]> FindSimilarAsync(Guid faceId, Guid[] faceIds, int maxNumOfCandidatesReturned = 20)
        {
            return innerClient.FindSimilarAsync(faceId, faceIds, maxNumOfCandidatesReturned);
        }

        public Task<SimilarFace[]> FindSimilarAsync(Guid faceId, Guid[] faceIds, FindSimilarMatchMode mode, int maxNumOfCandidatesReturned = 20)
        {
            return innerClient.FindSimilarAsync(faceId, faceIds, mode, maxNumOfCandidatesReturned);
        }

        public Task<SimilarPersistedFace[]> FindSimilarAsync(Guid faceId, string faceListId, int maxNumOfCandidatesReturned = 20)
        {
            return innerClient.FindSimilarAsync(faceId, faceListId, maxNumOfCandidatesReturned);
        }

        public Task<SimilarPersistedFace[]> FindSimilarAsync(Guid faceId, string faceListId, FindSimilarMatchMode mode, int maxNumOfCandidatesReturned = 20)
        {
            return innerClient.FindSimilarAsync(faceId, faceListId, mode, maxNumOfCandidatesReturned);
        }

        public Task<FaceList> GetFaceListAsync(string faceListId)
        {
            return innerClient.GetFaceListAsync(faceListId);
        }

        public Task<Person> GetPersonAsync(string personGroupId, Guid personId)
        {
            return RateLimitAwareCall(() => innerClient.GetPersonAsync(personGroupId, personId));
        }

        public Task<PersonFace> GetPersonFaceAsync(string personGroupId, Guid personId, Guid persistedFaceId)
        {
            return RateLimitAwareCall(() => innerClient.GetPersonFaceAsync(personGroupId, personId, persistedFaceId));
        }

        public Task<PersonGroup> GetPersonGroupAsync(string personGroupId)
        {
            return RateLimitAwareCall(() => innerClient.GetPersonGroupAsync(personGroupId));
        }

        public Task<PersonGroup[]> GetPersonGroupsAsync()
        {
            return RateLimitAwareCall(() => innerClient.ListPersonGroupsAsync());
        }

        public Task<PersonGroup[]> ListPersonGroupsAsync(string start = "", int top = 1000)
        {
            return RateLimitAwareCall(() => innerClient.ListPersonGroupsAsync(start, top));
        }

        public Task<TrainingStatus> GetPersonGroupTrainingStatusAsync(string personGroupId)
        {
            return RateLimitAwareCall(() => innerClient.GetPersonGroupTrainingStatusAsync(personGroupId));
        }

        public Task<Person[]> GetPersonsAsync(string personGroupId)
        {
            return RateLimitAwareCall(() => innerClient.GetPersonsAsync(personGroupId));
        }

        public Task<Person[]> ListPersonsAsync(string personGroupId, string start = "", int top = 1000)
        {
            return RateLimitAwareCall(() => innerClient.ListPersonsAsync(personGroupId, start, top));
        }

        public Task<GroupResult> GroupAsync(Guid[] faceIds)
        {
            return RateLimitAwareCall(() => innerClient.GroupAsync(faceIds));
        }

        public Task<IdentifyResult[]> IdentifyAsync(string personGroupId, Guid[] faceIds, int maxNumOfCandidatesReturned)
        {
            return RateLimitAwareCall(() => innerClient.IdentifyAsync(personGroupId, faceIds, maxNumOfCandidatesReturned));
        }

        public Task<IdentifyResult[]> IdentifyAsync(string personGroupId, Guid[] faceIds, float confidenceThreshold, int maxNumOfCandidatesReturned = 1)
        {
            return RateLimitAwareCall(() => innerClient.IdentifyAsync(personGroupId, faceIds, confidenceThreshold, maxNumOfCandidatesReturned));
        }

        public Task<FaceListMetadata[]> ListFaceListsAsync()
        {
            return RateLimitAwareCall(() => innerClient.ListFaceListsAsync());
        }

        public Task TrainPersonGroupAsync(string personGroupId)
        {
            return RateLimitAwareCall(
                async () =>
                {
                    await innerClient.TrainPersonGroupAsync(personGroupId);
                    return Unit.Default;
                });
        }

        public Task UpdateFaceListAsync(string faceListId, string name, string userData)
        {
            return innerClient.UpdateFaceListAsync(faceListId, name, userData);
        }

        public Task UpdatePersonAsync(string personGroupId, Guid personId, string name, string userData = null)
        {
            return innerClient.UpdatePersonAsync(personGroupId, personId, name, userData);
        }

        public Task UpdatePersonFaceAsync(string personGroupId, Guid personId, Guid persistedFaceId, string userData = null)
        {
            return innerClient.UpdatePersonFaceAsync(personGroupId, personId, persistedFaceId, userData);
        }

        public Task UpdatePersonGroupAsync(string personGroupId, string name, string userData = null)
        {
            return innerClient.UpdatePersonGroupAsync(personGroupId, name, userData);
        }

        public Task<VerifyResult> VerifyAsync(Guid faceId1, Guid faceId2)
        {
            return innerClient.VerifyAsync(faceId1, faceId2);
        }

        public Task<VerifyResult> VerifyAsync(Guid faceId, string personGroupId, Guid personId)
        {
            return innerClient.VerifyAsync(faceId, personGroupId, personId);
        }

        public HttpRequestHeaders DefaultRequestHeaders => innerClient.DefaultRequestHeaders;

        private static async Task<T> RateLimitAwareCall<T>(Func<Task<T>> call)
        {
            return await Observable
                .FromAsync(call)
                .RetryWithBackoffStrategy(retryOnError: IsRateLimitExceeded);
        }

        private static bool IsRateLimitExceeded(Exception arg)
        {
            if (arg is FaceAPIException faceApiException && faceApiException.ErrorCode == "RateLimitExceeded")
                return true;

            return false;
        }
    }
}