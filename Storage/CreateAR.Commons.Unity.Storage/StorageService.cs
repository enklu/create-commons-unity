using System;
using CreateAR.Commons.Unity.Async;
using CreateAR.Commons.Unity.Http;

using Void = CreateAR.Commons.Unity.Async.Void;

namespace CreateAR.Commons.Unity.Storage
{
    public class StorageWorker
    {
        private readonly IHttpService _http;

        public StorageWorker(IHttpService http)
        {
            _http = http;
        }
    }

    public class StorageWorkerResponse<T>
    {
        public T Value;
        public int Version;
    }

    public interface IStorageWorker
    {
        IAsyncToken<StorageWorkerResponse<T>> Load<T>(string key);
        IAsyncToken<StorageWorkerResponse<T>> Save<T>(string key, T value);
        IAsyncToken<Void> Delete<T>(string key);
    }
    
    public class StorageManifest
    {
        private const string KEYS_ENDPOINT = "user/{userId}/kv";
        
        private readonly IHttpService _http;

        public StorageManifest(IHttpService http)
        {
            _http = http;
        }

        public IAsyncToken<StorageManifest> Refresh()
        {
            throw new NotImplementedException();
        }

        public StorageBucket<T> FindOne<T>()
        {
            throw new NotImplementedException();
        }

        public StorageBucket<T>[] FindAll<T>()
        {
            throw new NotImplementedException();
        }
    }

    public interface IStorageService
    {
        StorageManifest Manifest { get; }

        IAsyncToken<StorageBucket<T>> Create<T>(T value);
    }

    public sealed class StorageService
    {

    }
}